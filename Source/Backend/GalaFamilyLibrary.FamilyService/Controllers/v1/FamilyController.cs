﻿using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using GalaFamilyLibrary.DataTransferObject.FamilyLibrary;
using GalaFamilyLibrary.FamilyService.Helpers;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Model.FamilyLibrary;
using GalaFamilyLibrary.Repository;
using GalaFamilyLibrary.Service.FamilyLibrary;
using GalaFamilyLibrary.Service.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Controllers.v1;

[ApiVersion("1.0")]
[Route("family/v{version:apiVersion}")]
public class FamilyController(
    IMinioClient minioClient,
    ILogger<FamilyController> logger,
    IRedisBasketRepository redis,
    IMapper mapper,
    IFamilyService familyService,
    RedisRequirement redisRequirement)
    : GalaControllerBase
{
    private readonly IMinioClient _minioClient = minioClient.WithRegion(_region);
    private const string _bucketName = "storage";
    private const string _region = "Shanghai";
    private const int _expiry = 60;

    private readonly Dictionary<string, string> _dirMap = new()
    {
        { "rfa", "family" },
        { "png", "image" },
        { "jpg", "image" },
        { "glb", "glb" }
    };

    [HttpGet("file/{id:long}/{familyVersion:int}")]
    public async Task<IActionResult> DownloadFamilyAsync(long id, ushort familyVersion)
    {
        var family = await familyService.GetByIdAsync(id);
        if (family is null)
        {
            return Ok("family not exist");
        }

        var file = family.GetFilePath(familyVersion);
        var fileUrl = await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(_bucketName).WithObject(file).WithExpiry(_expiry));
        return Redirect(fileUrl);
    }

    [HttpPost]
    public async Task<MessageData<string>> GetUploadUrlAsync(FamilyCreationDTO familyCreationDTO)
    {
        var lockKey = $"{familyCreationDTO.Name}{familyCreationDTO.UploaderId}";
        if (await redis.Exist(lockKey))
        {
            return Failed<string>("invalid request");
        }

        await redis.Set(lockKey, familyCreationDTO, TimeSpan.FromSeconds(5));
        var family = mapper.Map<Family>(familyCreationDTO);
        var args = new PresignedPutObjectArgs().WithBucket(_bucketName)
            .WithObject(family.GetFilePath(familyCreationDTO.Version)).WithExpiry(_expiry);
        var url = await _minioClient.PresignedPutObjectAsync(args);
        return Success(url);
    }

    [HttpPost("file")]
    public async Task<MessageData<string>> UploadFileAsync([FromForm] string fileName)
    {
        var lockKey = $"family/files/{fileName}";
        if (await redis.Exist(lockKey))
        {
            return Failed<string>("invalid request");
        }

        await redis.Set(lockKey, fileName, TimeSpan.FromSeconds(5));
        var suffix = Path.GetExtension(fileName)?.ToLower();
        if (!_dirMap.TryGetValue(suffix, out var dir))
        {
            return Failed<string>("invalid file format");
        }

        var args = new PresignedPutObjectArgs().WithBucket(_bucketName).WithObject($"{dir}/{fileName}")
            .WithExpiry(_expiry * 5);
        var url = await _minioClient.PresignedPutObjectAsync(args);
        return Success(url);

    }


    [HttpGet("details/{id:long}")]
    [AllowAnonymous]
    public async Task<MessageData<FamilyDetailDTO>> GetFamilyDetailAsync(long id)
    {
        var redisKey = $"familyDetails/{id}";
        if (await redis.Exist(redisKey))
        {
            return Success(await redis.Get<FamilyDetailDTO>(redisKey));
        }

        var familyDetail = await familyService.GetFamilyDetails(id);
        if (familyDetail is null)
        {
            logger.LogWarning("query family details failed id: {id} ,family not existed", id);
            return Failed<FamilyDetailDTO>("family not exist", 404);
        }

        logger.LogInformation("query family details succeed id: {id}", id);
        var detailDto = mapper.Map<FamilyDetailDTO>(familyDetail);
        await redis.Set(redisKey, detailDto, redisRequirement.CacheTime);
        return Success(detailDto);
    }

    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<MessageData<List<FamilyCategoryDTO>>> GetCategoriesAsync([FromQuery] int? parentId = null)
    {
        logger.LogInformation("query child categories by parent {parentId}", parentId);
        var redisKey = parentId == null ? $"family/categories" : $"family/categories?parentId={parentId}";
        if (await redis.Exist(redisKey))
        {
            return Success(await redis.Get<List<FamilyCategoryDTO>>(redisKey));
        }

        var categories = await familyService.GetCategoryTreeAsync(parentId);
        var categoriesDto = mapper.Map<List<FamilyCategoryDTO>>(categories);
        await redis.Set(redisKey, categoriesDto, TimeSpan.FromDays(1));
        return Success(categoriesDto);
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<MessageData<PageData<FamilyDTO>>> GetFamiliesPageAsync(string? keyword = null,
        long? categoryId = null, int pageIndex = 1, int pageSize = 30, string? order = "name")
    {
        logger.LogInformation(
            "query families by category {category} and keyword {keyword} at page {page} pageSize {pageSize}",
            categoryId, keyword, pageIndex, pageSize);
        var expression = Expressionable.Create<Family>()
            .AndIF(categoryId != null, f => f.CategoryId == categoryId)
            .AndIF(keyword != null, f => f.Name!.Contains(keyword))
            .ToExpression();

        var familyPage = await familyService.GetFamilyPageAsync(expression, pageIndex, pageSize, order);
        var args = new PresignedGetObjectArgs().WithBucket(_bucketName).WithExpiry(_expiry);
        familyPage.Data.ForEach(f => _minioClient.PresignedGetObjectAsync(args.WithObject(f.GetImagePath())));
        var familyPageDto = familyPage.ConvertTo<FamilyDTO>(mapper);
        return SucceedPage(familyPageDto);
    }
}