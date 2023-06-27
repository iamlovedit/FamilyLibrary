using AutoMapper;
using GalaFamilyLibrary.FamilyService.DataTransferObjects;
using GalaFamilyLibrary.FamilyService.Helpers;
using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Controllers.v1;

[ApiVersion("1.0")]
[Route("family/v{version:apiVersion}")]
public class FamilyController : ApiControllerBase
{
    private readonly IFamilyService _familyService;
    private readonly IFamilyCategoryService _categoryService;
    private readonly ILogger<FamilyController> _logger;
    private readonly IMapper _mapper;
    private readonly IRedisBasketRepository _redis;
    private readonly RedisRequirement _redisRequirement;
    private readonly FileStorageClient _fileStorageClient;

    public FamilyController(IFamilyService familyService, IFamilyCategoryService categoryService,
        ILogger<FamilyController> logger, IMapper mapper, IRedisBasketRepository redis,
        RedisRequirement redisRequirement, FileStorageClient fileStorageClient)
    {
        _familyService = familyService;
        _categoryService = categoryService;
        _logger = logger;
        _mapper = mapper;
        _redis = redis;
        _redisRequirement = redisRequirement;
        _fileStorageClient = fileStorageClient;
    }

    [HttpGet]
    [Route("{id:int}/{familyVersion:int}")]
    public Task<IActionResult> DownloadFamilyAsync(int id, ushort familyVersion)
    {
        return Task.Run<IActionResult>(async () =>
        {
            var redisKey = $"family/{id}";
            var family = default(Family);
            if (await _redis.Exist(redisKey))
            {
                family = await _redis.Get<Family>(redisKey);
            }
            else
            {
                family = await _familyService.GetByIdAsync(id);
                await _redis.Set(redisKey, family, _redisRequirement.CacheTime);
            }

            if (family is null)
            {
                _logger.LogWarning("download family by id: {id} version:{version} failed", id, familyVersion);
                return NotFound("file not exist");
            }

            var familyPath = family.GetFilePath(familyVersion);
            var url = _fileStorageClient.GetFileUrl(family.Name, familyPath);
            _logger.LogInformation("download family by id: {id} version:{version} succeed,file url is {url}", id,
                familyVersion, url);
            return Redirect(url);
        });
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<MessageModel<FamilyDTO>> GetFamilyDetailAsync(int id)
    {
        var redisKey = $"familyDetails/{id}";
        if (await _redis.Exist(redisKey))
        {
            return Success(await _redis.Get<FamilyDTO>(redisKey));
        }
        var family = await GetFamilyDetailAsync(id);
        if (family is null)
        {
            _logger.LogWarning("query family details failed id: {id} ,family not existed", id);
            return Failed<FamilyDTO>("family not exist", 404);
        }
        _logger.LogInformation("query family details succeed id: {id}", id);
        var familyDto = _mapper.Map<FamilyDTO>(family);
        await _redis.Set(redisKey, familyDto, _redisRequirement.CacheTime);
        return Success(familyDto);
    }


    [HttpGet]
    [Route("uploadUrl")]
    public async Task<MessageModel<Dictionary<string, string>>> GetUploadUrlAsync(
        [FromBody] FamilyCreationDTO familyCreationDto)
    {
        var family = _mapper.Map<Family>(familyCreationDto);
        var redisKey = $"family/{family.FileId}";
        var dictionary = default(Dictionary<string, string>);
        if (await _redis.Exist(redisKey))
        {
            dictionary = await _redis.Get<Dictionary<string, string>>(redisKey);
        }
        else
        {
            var familyUrl = _fileStorageClient.GetFileUrl(family.Name, family.GetFilePath(familyCreationDto.Version));
            var imageUrl = _fileStorageClient.GetFileUrl(family.Name, family.GetImagePath());
            dictionary = new Dictionary<string, string>
            {
                { "family", familyUrl },
                { "image", imageUrl },
                { "fileId", family.FileId }
            };
            await _redis.Set(redisKey, dictionary, TimeSpan.FromMinutes(5));
        }

        _logger.LogInformation("create upload url succeed,file id {fileId}", family.FileId);
        return Success(dictionary);
    }

    [HttpGet]
    [Route("categories")]
    [AllowAnonymous]
    public async Task<MessageModel<List<FamilyCategoryDTO>>> GetCategoriesAsync([FromQuery] int? parentId = null)
    {
        _logger.LogInformation("query child categories by parent {parentId}", parentId);
        var redisKey = parentId == null ? $"family/categories" : $"family/categories?parentId={parentId}";
        if (await _redis.Exist(redisKey))
        {
            return Success(await _redis.Get<List<FamilyCategoryDTO>>(redisKey));
        }
        var categories = await _categoryService.GetCategoryTreeAsync(parentId);
        var categoriesDto = _mapper.Map<List<FamilyCategoryDTO>>(categories);
        await _redis.Set(redisKey, categoriesDto, TimeSpan.FromDays(1));
        return Success(categoriesDto);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<MessageModel<PageModel<FamilyDTO>>> GetFamiliesPageAsync(
        [FromQuery] CategoryKeywordQuery categoryKeywordQuery)
    {
        var redisKey =
            $"families?keyword={categoryKeywordQuery.Keyword ?? "null"}&categoryId={categoryKeywordQuery.CategoryId}&pageIndex={categoryKeywordQuery.PageIndex}" +
            $"&pageSize={categoryKeywordQuery.PageSize}&orderField={categoryKeywordQuery.OrderField}";
        if (await _redis.Exist(redisKey))
        {
            return SucceedPage(await _redis.Get<PageModel<FamilyDTO>>(redisKey));
        }
        _logger.LogInformation("query families by category {category} and keyword {keyword} at page {page}",
            categoryKeywordQuery.CategoryId, categoryKeywordQuery.Keyword, categoryKeywordQuery.PageIndex);
        var expression = Expressionable.Create<Family>()
            .AndIF(categoryKeywordQuery.CategoryId != null, f => f.CategoryId == categoryKeywordQuery.CategoryId)
            .AndIF(categoryKeywordQuery.Keyword != null, f => f.Name.Contains(categoryKeywordQuery.Keyword))
            .ToExpression();

        var familyPage = await _familyService.QueryPageAsync(expression, categoryKeywordQuery.PageIndex, categoryKeywordQuery.PageSize, $"{categoryKeywordQuery.OrderField} DESC");
        var familyPageDto = familyPage.ConvertTo<FamilyDTO>(_mapper);
        await _redis.Set(redisKey, familyPageDto, _redisRequirement.CacheTime);
        return SucceedPage(familyPageDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] FamilyCallbackCreationDTO familyCreation)
    {
        try
        {
            var family = _mapper.Map<Family>(familyCreation);
            var id = await _familyService.AddAsync(family);
            if (id > 0)
            {
                _logger.LogInformation("create family succeed,family id {familyId}", family.Id);
                return Ok();
            }
            _logger.LogWarning("create family failed,file id {fileId}", family.FileId);
            return Problem();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(e.Message);
        }
    }
}