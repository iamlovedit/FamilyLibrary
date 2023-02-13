using AutoMapper;
using GalaFamilyLibrary.DynamoPackageService.DataTransferObjects;
using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v1;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}")]
public class PackageController : ApiControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ILogger<PackageController> _logger;
    private readonly IRedisBasketRepository _redis;
    private readonly IMapper _mapper;
    private readonly RedisRequirement _redisRequirement;

    public PackageController(IPackageService packageService, ILogger<PackageController> logger,
        IRedisBasketRepository redis, IMapper mapper, RedisRequirement redisRequirement)
    {
        _packageService = packageService;
        _logger = logger;
        _redis = redis;
        _mapper = mapper;
        _redisRequirement = redisRequirement;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<MessageModel<PackageDTO>> GetPackageAsync(string id)
    {
        var redisKey = $"packages/{id}";
        if (await _redis.Exist(redisKey))
        {
            return Success(await _redis.Get<PackageDTO>(redisKey));
        }
        else
        {
            var package = await _packageService.GetDynamoPackageByIdAsync(id);
            if (package != null)
            {
                _logger.LogInformation("query packages details by id {id}", id);
                var packageDTO = _mapper.Map<PackageDTO>(package);
                await _redis.Set(redisKey, packageDTO, _redisRequirement.CacheTime);
                return Success(packageDTO);
            }
        }

        return Failed<PackageDTO>("not found");
    }

    [HttpGet]
    [Route("{id}/{packageVersion}")]
    public async Task<IActionResult> Download(string id, string packageVersion)
    {
        _logger.LogInformation("download package id:{id} version:{packageVersion}", id, packageVersion);
        return await Task.FromResult(Redirect($"https://dynamopackages.com/download/{id}/{packageVersion}"));
    }

    [HttpGet]
    [Route("packages")]
    public async Task<MessageModel<PageModel<PackageDTO>>> GetPackagesByPage(string keyword = "", int pageIndex = 1,
        int pageSize = 30, string orderField = "")
    {
        var redisKey = $"packages?keyword={keyword}&pageIndex={pageIndex}&pageSize={pageSize}&orderField={orderField}";
        _logger.LogInformation("query packages at page:{page} pageSize:{pageSize} keyword:{keyword}", pageIndex,
            pageSize, keyword);
        if (await _redis.Exist(redisKey))
        {
            return SucceedPage(await _redis.Get<PageModel<PackageDTO>>(redisKey));
        }
        else
        {
            var hasKeyword = string.IsNullOrEmpty(keyword);
            var hasField = string.IsNullOrEmpty(orderField);
            var packagesPage = await _packageService.QueryPageAsync(
                hasKeyword ? null : p => p.Name.Contains(keyword) && !p.IsDeleted, pageIndex, pageSize,
                hasField ? "" : $"{orderField} desc");
            var result = packagesPage.ConvertTo<PackageDTO>(_mapper);
            await _redis.Set(redisKey, result, _redisRequirement.CacheTime);
            return SucceedPage(result);
        }
    }
}