using AutoMapper;
using GalaFamilyLibrary.FamilyService.DataTransferObjects;
using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.FamilyService.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
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
        var family = await _familyService.GetByIdAsync(id);

        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "families", $"{familyVersion}",
            $"{family.FileId}");
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            var buffer = new byte[fileStream.Length];
            await fileStream.ReadAsync(buffer, 0, buffer.Length);
            return File(buffer, family.Name);
        }
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    [AllowAnonymous]
    public async Task<MessageModel<List<FamilyCategoryDTO>>> GetCategoriesAsync(int id = 0)
    {
        _logger.LogInformation("query all family categories");
        var redisKey = $"family/categories/{id}";
        if (await _redis.Exist(redisKey))
        {
            return Success(await _redis.Get<List<FamilyCategoryDTO>>(redisKey));
        }
        else
        {
            var categories = await _categoryService.GetCategoryTreeAsync(id);
            var categoryDtos = _mapper.Map<List<FamilyCategoryDTO>>(categories);
            await _redis.Set(redisKey, categoryDtos, TimeSpan.FromDays(1));
            return Success(categoryDtos);
        }
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<MessageModel<PageModel<FamilyDTO>>> GetFamiliesPageAsync(string keyword = "", int pageIndex = 1,
        int pageSize = 30, string orderField = "")
    {
        var redisKey = $"families?keyword={keyword}&pageIndex={pageIndex}&pageSize={pageSize}";
        if (await _redis.Exist(redisKey))
        {
            return SucceedPage(await _redis.Get<PageModel<FamilyDTO>>(redisKey));
        }
        else
        {
            var familyPage = await _familyService.QueryPageAsync(
                string.IsNullOrEmpty(keyword) ? null : f => f.Name.Contains(keyword),
                pageIndex, pageSize, string.IsNullOrEmpty(orderField) ? "" : $"{orderField} DESC");
            var familyPageDTO = familyPage.ConvertTo<FamilyDTO>(_mapper);
            await _redis.Set(redisKey, familyPageDTO, _redisRequirement.CacheTime);
            return SucceedPage(familyPageDTO);
        }
    }

    [HttpPost("upload")]
    [Authorize]
    public async Task<MessageModel<int>> CreateFamilyAsync([FromBody] FamilyCreationDTO familyCreationDto)
    {
        var family = _mapper.Map<Family>(familyCreationDto);
        var id = await _familyService.AddAsync(family);
        if (id > 0)
        {
            family.Id = id;
            await _redis.Set($"family/{id}", family, TimeSpan.FromMinutes(30));
            return Success(id);
        }
        else
        {
            return Failed<int>("上传失败");
        }
    }
}