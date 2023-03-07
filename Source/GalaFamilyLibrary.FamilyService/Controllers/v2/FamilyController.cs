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

namespace GalaFamilyLibrary.FamilyService.Controllers.v2;

[ApiVersion("2.0")]
[Route("family/v{version:apiVersion}")]
public class FamilyController : ApiControllerBase
{
    private readonly IFamilyService _familyService;
    private readonly IFamilyCategoryService _categoryService;
    private readonly ILogger<FamilyController> _logger;
    private readonly IMapper _mapper;
    private readonly IRedisBasketRepository _redis;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly RedisRequirement _redisRequirement;
    private readonly FileStorageClient _fileStorageClient;

    public FamilyController(IFamilyService familyService, IFamilyCategoryService categoryService,
        ILogger<FamilyController> logger, IMapper mapper, IRedisBasketRepository redis,
        IWebHostEnvironment webHostEnvironment, RedisRequirement redisRequirement, FileStorageClient fileStorageClient)
    {
        _familyService = familyService;
        _categoryService = categoryService;
        _logger = logger;
        _mapper = mapper;
        _redis = redis;
        _redisRequirement = redisRequirement;
        _webHostEnvironment = webHostEnvironment;
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
                return NotFound("file not exist");
            }
            var familyPath = family.GetFilePath(familyVersion);
            var url = _fileStorageClient.GetFileUrl(family.Name, familyPath);
            return Redirect(url);
        });
    }

    [HttpGet]
    [Route("uploadUrl")]
    public async Task<MessageModel<Dictionary<string, string>>> GetUploadUrlAsync([FromBody] FamilyCreationDTO familyCreationDto)
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
                    {"family",familyUrl },
                    {"image", imageUrl},
                    {"fileId",family.FileId }
                };
            await _redis.Set(redisKey, dictionary, TimeSpan.FromMinutes(5));
        }
        return Success(dictionary);
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    [AllowAnonymous]
    public async Task<MessageModel<List<FamilyCategoryDTO>>> GetCategoriesAsync(int id = 0)
    {
        _logger.LogInformation("query child categories by parent {parentId}", id);
        var redisKey = $"family/categories/{id}";
        if (await _redis.Exist(redisKey))
        {
            return Success(await _redis.Get<List<FamilyCategoryDTO>>(redisKey));
        }
        else
        {
            var categories = await _categoryService.GetCategoryTreeAsync(id);
            var categoriesDto = _mapper.Map<List<FamilyCategoryDTO>>(categories);
            await _redis.Set(redisKey, categoriesDto, TimeSpan.FromDays(1));
            return Success(categoriesDto);
        }
    }

    [HttpGet]
    [Route("keyword")]
    [AllowAnonymous]
    public async Task<MessageModel<PageModel<FamilyDTO>>> GetFamiliesPageAsync([FromQuery] KeywordQuery keywordQuery)
    {
        var redisKey =
            $"families?keyword={keywordQuery.Keyword}&pageIndex={keywordQuery.PageIndex}&pageSize={keywordQuery.PageSize}&orderField={keywordQuery.OrderField}";
        if (await _redis.Exist(redisKey))
        {
            return SucceedPage(await _redis.Get<PageModel<FamilyDTO>>(redisKey));
        }
        else
        {
            var familyPage = await _familyService.QueryPageAsync(
                string.IsNullOrEmpty(keywordQuery.Keyword) ? null : f => f.Name.Contains(keywordQuery.Keyword),
                keywordQuery.PageIndex, keywordQuery.PageSize, $"{keywordQuery.OrderField} DESC");
            var familyPageDto = familyPage.ConvertTo<FamilyDTO>(_mapper);
            await _redis.Set(redisKey, familyPageDto, _redisRequirement.CacheTime);
            return SucceedPage(familyPageDto);
        }
    }

    [HttpGet]
    [Route("category")]
    [AllowAnonymous]
    public async Task<MessageModel<PageModel<FamilyDTO>>> GetFamiliesPageAsync([FromQuery] CategoryQuery categoryQuery)
    {
        var redisKey =
            $"families?categoryId={categoryQuery.CategoryId}&pageIndex={categoryQuery.PageIndex}&pageSize={categoryQuery.PageSize}&orderField={categoryQuery.OrderField}";
        if (await _redis.Exist(redisKey))
        {
            return SucceedPage(await _redis.Get<PageModel<FamilyDTO>>(redisKey));
        }
        else
        {
            var familyPage = await _familyService.QueryPageAsync(
                categoryQuery.CategoryId == 0 ? null : f => f.CategoryId == categoryQuery.CategoryId,
                categoryQuery.PageIndex, categoryQuery.PageSize, $"{categoryQuery.OrderField} DESC");
            var familyPageDto = familyPage.ConvertTo<FamilyDTO>(_mapper);
            await _redis.Set(redisKey, familyPageDto, _redisRequirement.CacheTime);
            return SucceedPage(familyPageDto);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<MessageModel<PageModel<FamilyDTO>>> GetFamiliesPageAsync([FromQuery] CategoryKeywordQuery categoryKeywordQuery)
    {
        var redisKey =
            $"families?keyword={categoryKeywordQuery.Keyword}&categoryId={categoryKeywordQuery.CategoryId}&pageIndex={categoryKeywordQuery.PageIndex}" +
            $"&pageSize={categoryKeywordQuery.PageSize}&orderField={categoryKeywordQuery.OrderField}";
        if (await _redis.Exist(redisKey))
        {
            return SucceedPage(await _redis.Get<PageModel<FamilyDTO>>(redisKey));
        }
        else
        {
            var familyPage = await _familyService.QueryPageAsync(
                f => f.Name.Contains(categoryKeywordQuery.Keyword) && f.CategoryId == categoryKeywordQuery.CategoryId,
                categoryKeywordQuery.PageIndex, categoryKeywordQuery.PageSize,
                $"{categoryKeywordQuery.OrderField} DESC");
            var familyPageDto = familyPage.ConvertTo<FamilyDTO>(_mapper);
            await _redis.Set(redisKey, familyPageDto, _redisRequirement.CacheTime);
            return SucceedPage(familyPageDto);
        }
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
                return Ok();
            }
            else
            {
                return Problem();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Problem(e.Message);
        }
    }
}