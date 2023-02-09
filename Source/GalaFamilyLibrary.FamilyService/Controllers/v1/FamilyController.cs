using AutoMapper;
using GalaFamilyLibrary.FamilyService.DataTransferObjects;
using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
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

    public FamilyController(IFamilyService familyService, IFamilyCategoryService categoryService,
        ILogger<FamilyController> logger, IMapper mapper, IRedisBasketRepository redis)
    {
        _familyService = familyService;
        _categoryService = categoryService;
        _logger = logger;
        _mapper = mapper;
        _redis = redis;
    }

    [HttpGet]
    [Route("categories")]
    public async Task<MessageModel<List<FamilyCategoryDTO>>> GetCategoriesAsync()
    {
        _logger.LogInformation("query all family categories");
        var redisKey = "family/categories";
        if (await _redis.Exist(redisKey))
        {
            return Success(await _redis.Get<List<FamilyCategoryDTO>>(redisKey));
        }
        else
        {
            var categories = await _categoryService.GetCategoryTreeAsync();
            var categoryDtos = _mapper.Map<List<FamilyCategoryDTO>>(categories);
            await _redis.Set(redisKey, categoryDtos, TimeSpan.FromDays(1));
            return Success(categoryDtos);
        }
    }

    [HttpPost]
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