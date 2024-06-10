using AutoMapper;
using FluentValidation;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary;
using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Domain.Validators;
using GalaFamilyLibrary.FamilyService.Helpers;
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
public class FamilyController(
    IFamilyService familyService,
    IFamilyCategoryService categoryService,
    ILogger<FamilyController> logger,
    IMapper mapper,
    IRedisBasketRepository redis,
    IValidator<FamilyCreationValidator> familyCreationValidator,
    RedisRequirement redisRequirement,
    FileStorageClient fileStorageClient)
    : GalaControllerBase
{
    [HttpGet]
    [Route("{id:long}/{familyVersion:int}")]
    public Task<IActionResult> DownloadFamilyAsync(long id, ushort familyVersion)
    {
        return Task.Run<IActionResult>(async () =>
        {
            var family = await familyService.GetByIdAsync(id);
            if (family is null)
            {
                logger.LogWarning("download family by id: {id} version:{version} failed", id, familyVersion);
                return NotFound("file not exist");
            }

            var familyPath = family.GetFilePath(familyVersion);
            var url = fileStorageClient.GetFileUrl(family.Name, familyPath);
            family.Downloads += 1;
            await familyService.UpdateColumnsAsync(family, f => f.Downloads);
            logger.LogInformation("download family by id: {id} version:{version} succeed,file url is {url}", id,
                familyVersion, url);
            return Redirect(url);
        });
    }

    [HttpGet]
    [Route("details/{id:long}")]
    public async Task<MessageModel<FamilyDetailDTO>> GetFamilyDetailAsync(long id)
    {
        var redisKey = $"familyDetails/{id}";
        if (await redis.Exist(redisKey))
        {
            return Success(await redis.Get<FamilyDetailDTO>(redisKey));
        }

        var family = await familyService.GetFamilyDetails(id);
        if (family is null)
        {
            logger.LogWarning("query family details failed id: {id} ,family not existed", id);
            return Failed<FamilyDetailDTO>("family not exist", 404);
        }

        logger.LogInformation("query family details succeed id: {id}", id);
        var familyDto = mapper.Map<FamilyDetailDTO>(family);
        await redis.Set(redisKey, familyDto, redisRequirement.CacheTime);
        return Success(familyDto);
    }


    [HttpGet]
    [Route("uploadUrl")]
    public async Task<MessageModel<Dictionary<string, string>>> GetUploadUrlAsync(
        [FromBody] FamilyCreationDTO familyCreationDto)
    {
        var family = mapper.Map<Family>(familyCreationDto);
        var redisKey = $"family/{family.FileId}";
        var dictionary = default(Dictionary<string, string>);
        if (await redis.Exist(redisKey))
        {
            dictionary = await redis.Get<Dictionary<string, string>>(redisKey);
        }
        else
        {
            var familyUrl = fileStorageClient.GetFileUrl(family.Name, family.GetFilePath(familyCreationDto.Version));
            var imageUrl = fileStorageClient.GetFileUrl(family.Name, family.GetImagePath());
            dictionary = new Dictionary<string, string>
            {
                { "family", familyUrl },
                { "image", imageUrl },
                { "fileId", family.FileId }
            };
            await redis.Set(redisKey, dictionary, TimeSpan.FromMinutes(5));
        }

        logger.LogInformation("create upload url succeed,file id {fileId}", family.FileId);
        return Success(dictionary);
    }

    [HttpGet]
    [Route("categories")]
    [AllowAnonymous]
    public async Task<MessageModel<List<FamilyCategoryDTO>>> GetCategoriesAsync([FromQuery] int? parentId = null)
    {
        logger.LogInformation("query child categories by parent {parentId}", parentId);
        var redisKey = parentId == null ? $"family/categories" : $"family/categories?parentId={parentId}";
        if (await redis.Exist(redisKey))
        {
            return Success(await redis.Get<List<FamilyCategoryDTO>>(redisKey));
        }

        var categories = await categoryService.GetCategoryTreeAsync(parentId);
        var categoriesDto = mapper.Map<List<FamilyCategoryDTO>>(categories);
        await redis.Set(redisKey, categoriesDto, TimeSpan.FromDays(1));
        return Success(categoriesDto);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<MessageModel<PageModel<FamilyBasicDTO>>> GetFamiliesPageAsync(
        [FromQuery] CategoryKeywordQuery categoryKeywordQuery)
    {
        var redisKey =
            $"families?keyword={categoryKeywordQuery.Keyword ?? "null"}&categoryId={categoryKeywordQuery.CategoryId}&pageIndex={categoryKeywordQuery.PageIndex}" +
            $"&pageSize={categoryKeywordQuery.PageSize}&orderField={categoryKeywordQuery.OrderField}";
        if (await redis.Exist(redisKey))
        {
            return SucceedPage(await redis.Get<PageModel<FamilyBasicDTO>>(redisKey));
        }

        logger.LogInformation("query families by category {category} and keyword {keyword} at page {page}",
            categoryKeywordQuery.CategoryId, categoryKeywordQuery.Keyword, categoryKeywordQuery.PageIndex);
        var expression = Expressionable.Create<Family>()
            .AndIF(categoryKeywordQuery.CategoryId != null, f => f.CategoryId == categoryKeywordQuery.CategoryId)
            .AndIF(categoryKeywordQuery.Keyword != null, f => f.Name.Contains(categoryKeywordQuery.Keyword))
            .ToExpression();

        var familyPage = await familyService.GetFamilyPageAsync(expression, categoryKeywordQuery.PageIndex,
            categoryKeywordQuery.PageSize, $"{categoryKeywordQuery.OrderField} DESC");
        var familyPageDto = familyPage.ConvertTo<FamilyBasicDTO>(mapper);
        await redis.Set(redisKey, familyPageDto, redisRequirement.CacheTime);
        return SucceedPage(familyPageDto);
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateAsync([FromForm] FamilyCallbackCreationDTO familyCreation)
    //{
    //    try
    //    {
    //        var family = _mapper.Map<Family>(familyCreation);
    //        var id = await _familyService.AddAsync(family);
    //        if (id > 0)
    //        {
    //            _logger.LogInformation("create family succeed,family id {familyId}", family.Id);
    //            return Ok();
    //        }

    //        _logger.LogWarning("create family failed,file id {fileId}", family.FileId);
    //        return Problem();
    //    }
    //    catch (Exception e)
    //    {
    //        _logger.LogError(e, e.Message);
    //        return Problem(e.Message);
    //    }
    //}
}