﻿using AutoMapper;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Xml;
using Asp.Versioning;
using GalaFamilyLibrary.DataTransferObject.Package;
using GalaFamilyLibrary.Model.Package;
using GalaFamilyLibrary.Repository;
using GalaFamilyLibrary.Repository.UnitOfWorks;
using GalaFamilyLibrary.Service.Package;
using SqlSugar;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v1;

[ApiVersion("1.0")]
[Route("package/v{version:apiVersion}")]
[AllowAnonymous]
public class PackageController(
    IPackageService packageService,
    ILogger<PackageController> logger,
    IRedisBasketRepository redis,
    IMapper mapper)
    : GalaControllerBase
{
    private readonly TimeSpan _cacheTime = TimeSpan.FromDays(1);

    [HttpGet("versions/{id}")]
    public async Task<MessageData<PageData<PackageVersionDTO>>> GetVersionAsync([FromRoute] string id,
        [FromServices] IVersionService versionService,
        [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
    {
        logger.LogInformation("query package versions by id {id}", id);
        var redisKey = $"package/versions/{id}?pageIndex={pageIndex}&pageSize={pageSize}";
        if (await redis.Exist(redisKey))
        {
            return SucceedPage(await redis.Get<PageData<PackageVersionDTO>>(redisKey));
        }

        var versionPage =
            await versionService.QueryPageAsync(pv => pv.PackageId == id, pageIndex, pageSize, pv => pv.CreatedDate,
                OrderByType.Desc);
        var result = versionPage.ConvertTo<PackageVersionDTO>(mapper);
        await redis.Set(redisKey, result, _cacheTime);
        return SucceedPage(result);
    }

    [HttpGet("detail/{id}")]
    public async Task<MessageData<PackageDTO>> GetPackageDescriptionAsync(string id)
    {
        var redisKey = $"package/detail/{id}";
        var package = default(DynamoPackage);
        if (await redis.Exist(redisKey))
        {
            package = await redis.Get<DynamoPackage>(redisKey);
            return Success(mapper.Map<PackageDTO>(package));
        }

        package = await packageService.GetPackageDetailByIdAsync(id);
        if (package is null)
        {
            return Failed<PackageDTO>("404", 404);
        }

        await redis.Set(redisKey, package, _cacheTime);
        return Success(mapper.Map<PackageDTO>(package));
    }

    [HttpGet("{id}/{packageVersion}")]
    public Task<IActionResult> Download(string id, string packageVersion)
    {
        return Task.Run<IActionResult>(() =>
        {
            logger.LogInformation("download package id:{id} version:{packageVersion}", id, packageVersion);
            return Redirect($"https://dynamopackages.com/download/{id}/{packageVersion}");
        });
    }

    [HttpGet("packages")]
    [AllowAnonymous]
    public async Task<MessageData<PageData<PackageDTO>>> GetPackagesByPage(string? keyword = null, int pageIndex = 1,
        int pageSize = 30, string? orderBy = null)
    {
        logger.LogInformation(
            "query packages by keyword: {keyword} pageIndex: {pageIndex} pageSize: {pageSize} orderBy: {orderBy}",
            keyword, pageIndex, pageSize, orderBy);
        var redisKey = $"packages?keyword={keyword}&pageIndex={pageIndex}&pageSize={pageSize}&orderBy={orderBy}";
        var packagesPage = default(PageData<DynamoPackage>);
        if (await redis.Exist(redisKey))
        {
            packagesPage = await redis.Get<PageData<DynamoPackage>>(redisKey);
        }
        else
        {
            var expression = Expressionable.Create<DynamoPackage>()
                .AndIF(!string.IsNullOrEmpty(keyword), p => p.Name!.Contains(keyword!)).ToExpression();
            packagesPage = await packageService.GetPackagePageAsync(expression, pageIndex, pageSize, orderBy);
            await redis.Set(redisKey, packagesPage, _cacheTime);
        }

        return SucceedPage(packagesPage.ConvertTo<PackageDTO>(mapper));
    }

    [HttpPost]
    [Authorize(Roles = PermissionConstants.ROLE_ADMINISTRATOR)]
    public async Task<MessageData> UpdateAsync([FromServices] IHttpClientFactory clientFactory,
        [FromServices] DatabaseContext appDbContext, [FromServices] IUnitOfWork unitOfWork)
    {
        var httpClient = clientFactory.CreateClient();
        var responseMessage = await httpClient.GetAsync("https://dynamopackages.com/packages");
        if (responseMessage.IsSuccessStatusCode)
        {
            var json = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var jObject = JObject.Parse(json);
                    var content = jObject["content"];
                    if (content is null)
                    {
                        return Failed("获取原网站数据失败");
                    }

                    var newPackages = content.ToObject<List<DynamoPackage>>();
                    var packageDb = appDbContext.GetEntityDatabase<DynamoPackage>();
                    var packageVersionDb = appDbContext.GetEntityDatabase<PackageVersion>();
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

                    logger.LogInformation("added new package count {added},added new version count {addedverson}",
                        addedPackages.Count, addedPackageVersions.Count);
                    unitOfWork.CommitTransaction();
                }
                catch (Exception e)
                {
                    unitOfWork.RollbackTransaction();
                    logger.LogError(e, e.Message);
                    return Failed(e.Message);
                }

                return Success("更新完成");
            }
        }

        return Failed($"request failed,http status code {responseMessage.StatusCode}");
    }

    [HttpPost("update")]
    [DisableRequestSizeLimit]
    [RequestSizeLimit(int.MaxValue)]
    [Authorize(Roles = PermissionConstants.ROLE_ADMINISTRATOR)]
    public async Task<MessageData> UpdateByPackagesAsync([FromServices] IUnitOfWork unitOfWork,
        [FromServices] DatabaseContext appDbContext, [FromBody] List<DynamoPackage> packages)
    {
        try
        {
            var packageDb = appDbContext.GetEntityDatabase<DynamoPackage>();
            var packageVersionDb = appDbContext.GetEntityDatabase<PackageVersion>();
            var oldPackages = await packageDb.GetListAsync();
            var oldPackageVersions = await packageVersionDb.GetListAsync();
            unitOfWork.BeginTransaction();
            var addedPackages = new List<DynamoPackage>();
            var addedPackageVersions = new List<PackageVersion>();
            var newPackageVersions = new List<PackageVersion>();
            foreach (var package in packages)
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
                var newPackage = packages.FirstOrDefault(p => p.Id == package.Id);
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

            logger.LogInformation("added new package count {added},added new version count {addedverson}",
                addedPackages.Count, addedPackageVersions.Count);
            unitOfWork.CommitTransaction();
        }
        catch (Exception e)
        {
            unitOfWork.RollbackTransaction();
            logger.LogError(e, e.Message);
            return Failed(e.Message);
        }

        return Success("更新完成");
    }
}