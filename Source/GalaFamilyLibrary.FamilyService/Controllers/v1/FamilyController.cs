using AutoMapper;
using GalaFamilyLibrary.FamilyService.DataTransferObjects;
using GalaFamilyLibrary.FamilyService.Helpers;
using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.FamilyService.Controllers.v1;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}")]
public class FamilyController : ApiControllerBase
{
    private readonly IFamilyService _familyService;
    private readonly IFamilyCategoryService _categoryService;
    private readonly ILogger<FamilyController> _logger;
    private readonly IMapper _mapper;
    private readonly IRedisBasketRepository _redis;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly RedisRequirement _redisRequirement;

    public FamilyController(IFamilyService familyService, IFamilyCategoryService categoryService,
        ILogger<FamilyController> logger, IMapper mapper, IRedisBasketRepository redis,
        IWebHostEnvironment webHostEnvironment,
        RedisRequirement redisRequirement)
    {
        _familyService = familyService;
        _categoryService = categoryService;
        _logger = logger;
        _mapper = mapper;
        _redis = redis;
        _redisRequirement = redisRequirement;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Route("{id:int}/{familyVersion:int}")]
    public async Task<IActionResult> DownloadFamilyAsync(int id, ushort familyVersion)
    {
        var redisKey = $"family/{id}/{familyVersion}";
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

        if (family?.IsDeleted ?? true)
        {
            return NotFound("family not exist");
        }

        var filePath = family.GetFilePath(_webHostEnvironment, familyVersion);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(fileBytes, "application/stream", $"{family.Name}.rfa");
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
            familyPageDto.Data.ForEach(f =>
            {
                f.ImageUrl =f.GetImagePath(_webHostEnvironment);
            });
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
            familyPageDto.Data.ForEach(f =>
            {
                f.ImageUrl =f.GetImagePath(_webHostEnvironment);
            });
            await _redis.Set(redisKey, familyPageDto, _redisRequirement.CacheTime);
            return SucceedPage(familyPageDto);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<MessageModel<PageModel<FamilyDTO>>> GetFamiliesPageAsync(
        [FromQuery] CategoryKeywordQuery categoryKeywordQuery)
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
            familyPageDto.Data.ForEach(f =>
            {
                f.ImageUrl =f.GetImagePath(_webHostEnvironment);
            });
            await _redis.Set(redisKey, familyPageDto, _redisRequirement.CacheTime);
            return SucceedPage(familyPageDto);
        }
    }

    [HttpPost("upload")]
    public async Task<MessageModel<int>> CreateFamilyAsync(IFormFile familyFile, IFormFile imageFile,
        [FromForm] FamilyCreationDTO familyCreationDto)
    {
        var fileExtension = Path.GetExtension(familyFile.FileName);
        if (fileExtension.ToLower() != ".rfa")
        {
            return Failed<int>("error family file format");
        }

        var imageExtension = Path.GetExtension(imageFile.FileName);
        if (imageExtension.ToLower() != ".png")
        {
            return Failed<int>("error image file format");
        }

        using (var memoryStream = new MemoryStream())
        {
            await familyFile.OpenReadStream().CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            if (familyCreationDto.MD5 == fileBytes.EncryptMD5())
            {
                var family = _mapper.Map<Family>(familyCreationDto);
                await System.IO.File.WriteAllBytesAsync(
                    family.GetFilePath(_webHostEnvironment, familyCreationDto.Version), fileBytes);
                await memoryStream.FlushAsync();
                await imageFile.OpenReadStream().CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                await System.IO.File.WriteAllBytesAsync(family.GetImagePath(_webHostEnvironment), imageBytes);
                var id = await _familyService.AddAsync(family);
                if (id > 0)
                {
                    family.Id = id;
                    await _redis.Set($"family/{id}", family, TimeSpan.FromMinutes(30));
                    return Success(id);
                }
                else
                {
                    return Failed<int>("upload failed");
                }
            }
        }

        return Failed<int>("upload failed");
    }
}