using System.Linq.Expressions;
using System.Net.Http;
using AutoMapper;
using GalaFamilyLibrary.DynamoPackageService.DataTransferObjects;
using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v1;

[ApiVersion("1.0")]
[Route("package/v{version:apiVersion}")]
public class PackageController : ApiControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ILogger<PackageController> _logger;
    private readonly IRedisBasketRepository _redis;
    private readonly IMapper _mapper;
    private readonly TimeSpan _cacheTime;

    public PackageController(IPackageService packageService, ILogger<PackageController> logger,
        IRedisBasketRepository redis, IMapper mapper, RedisRequirement redisRequirement)
    {
        _packageService = packageService;
        _logger = logger;
        _redis = redis;
        _mapper = mapper;
        _cacheTime = TimeSpan.FromDays(1);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<MessageModel<PageModel<PackageVersionDTO>>> GetVersionAsync([FromRoute] string id,
        [FromServices] IVersionService versionService, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
    {
        _logger.LogInformation("query package versions by id {id}", id);
        var redisKey = $"package/versions/{id}?pageIndex={pageIndex}&pageSize={pageSize}";
        if (await _redis.Exist(redisKey))
        {
            return SucceedPage(await _redis.Get<PageModel<PackageVersionDTO>>(redisKey));
        }

        var versionPage =
            await versionService.QueryPageAsync(pv => pv.PackageId == id && !pv.IsDeleted, pageIndex, pageSize,
                "createTime desc");
        var result = versionPage.ConvertTo<PackageVersionDTO>(_mapper);
        await _redis.Set(redisKey, result, _cacheTime);
        return SucceedPage(result);
    }


    [HttpGet]
    [Route("{id}/{packageVersion}")]
    public Task<IActionResult> Download(string id, string packageVersion)
    {
        return Task.Run<IActionResult>(() =>
        {
            _logger.LogInformation("download package id:{id} version:{packageVersion}", id, packageVersion);
            return Redirect($"https://dynamopackages.com/download/{id}/{packageVersion}");
        });
    }

    [HttpGet]
    [Route("packages")]
    public async Task<MessageModel<PageModel<PackageDTO>>> GetPackagesByPage(string? keyword = null, int pageIndex = 1,
        int pageSize = 30, string? orderField = "downloads")
    {
        _logger.LogInformation(
            "query packages by keyword: {keyword} pageIndex: {pageIndex} pageSize: {pageSize} orderField: {orderField}",
            keyword,
            pageIndex, pageSize, orderField);
        var redisKey = $"?keyword={keyword}&pageIndex={pageIndex}&pageSize={pageSize}&orderField={orderField}";
        if (await _redis.Exist(redisKey))
        {
            return SucceedPage(await _redis.Get<PageModel<PackageDTO>>(redisKey));
        }

        Expression<Func<DynamoPackage, bool>> expression = string.IsNullOrEmpty(keyword)
            ? p => !p.IsDeleted
            : p => p.Name.Contains(keyword) && !p.IsDeleted;
        var packagesPage = await _packageService.QueryPageAsync(expression,
            pageIndex, pageSize, string.IsNullOrEmpty(orderField) ? null : $"{orderField} desc");
        var result = packagesPage.ConvertTo<PackageDTO>(_mapper);
        await _redis.Set(redisKey, result, _cacheTime);
        return SucceedPage(result);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<MessageModel<string>> UpdateAsync([FromServices] IHttpClientFactory clientFactory,
        [FromServices] AppDbContext appDbContext, [FromServices] IUnitOfWork unitOfWork)
    {
        var httpClient = clientFactory.CreateClient();
        var responseMessage = await httpClient.GetAsync("https://dynamopackages.com/packages");
        if (responseMessage.IsSuccessStatusCode)
        {
            var json = await responseMessage?.Content?.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var jObject = JObject.Parse(json);
                    var content = jObject["content"];
                    if (content is null)
                    {
                        return Failed<string>("获取原网站数据失败");
                    }

                    var newPackages = content.ToObject<List<DynamoPackage>>();
                    var packageDb = appDbContext.GetEntityDB<DynamoPackage>();
                    var packageVersionDb = appDbContext.GetEntityDB<PackageVersion>();
                    var oldPackages = await packageDb.GetListAsync();
                    var oldPackageVersions = await packageVersionDb.GetListAsync();
                    unitOfWork.BeginTransaction();
                    var addedPackages = new List<DynamoPackage>();
                    var addedPackageVersions = new List<PackageVersion>();
                    var newPackageVersions = new List<PackageVersion>();
                    foreach (var package in newPackages)
                    {
                        var oldPackage = oldPackages.FirstOrDefault(p => p.Id == package.Id);
                        if (oldPackage is null)
                        {
                            addedPackages.Add(package);
                        }
                        else
                        {
                            await packageDb.UpdateAsync(package);
                        }

                        foreach (var pVersion in package.Versions)
                        {
                            pVersion.PackageId = package.Id;
                            var oldPackageVersion = oldPackageVersions.FirstOrDefault(pv =>
                                pv.PackageId == package.Id && pv.Version == pVersion.Version);
                            if (oldPackageVersion is null)
                            {
                                addedPackageVersions.Add(pVersion);
                            }
                            else
                            {
                                await packageVersionDb.UpdateAsync(pVersion);
                            }

                            newPackageVersions.Add(pVersion);
                        }
                    }

                    await packageDb.InsertRangeAsync(addedPackages);
                    await packageVersionDb.InsertRangeAsync(addedPackageVersions);

                    foreach (var package in oldPackages)
                    {
                        var newPackage = newPackages.FirstOrDefault(p => p.Id == package.Id);
                        if (newPackage is null)
                        {
                            package.IsDeleted = true;
                            await packageDb.UpdateAsync(package);
                        }
                    }

                    foreach (var pVersion in oldPackageVersions)
                    {
                        var newVersion = newPackageVersions.FirstOrDefault(pv =>
                            pv.PackageId == pVersion.PackageId && pv.Version == pVersion.Version);
                        if (newVersion is null)
                        {
                            pVersion.IsDeleted = true;
                            await packageVersionDb.UpdateAsync(pVersion);
                        }
                    }

                    _logger.LogInformation("added new package count {added},added new version count {addedverson}",
                        addedPackages.Count, addedPackageVersions.Count);
                    unitOfWork.CommitTransaction();
                }
                catch (Exception e)
                {
                    unitOfWork.RollbackTransaction();
                    _logger.LogError(e, e.Message);
                    return Failed(e.Message);
                }

                return Success("更新完成");
            }
        }

        return Failed($"request failed,http status code {responseMessage.StatusCode}");
    }
}