using Asp.Versioning;
using GalaFamilyLibrary.DataTransferObject.Package;
using GalaFamilyLibrary.Infrastructure;
using GalaFamilyLibrary.Infrastructure.Exceptions;
using GalaFamilyLibrary.Service.Package;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v2;

[ApiVersion("2.0")]
[Route("package/v{version:apiVersion}")]
public class PackageController(IPublishedPackageService packageService, ILogger<PackageController> logger)
    : DefaultControllerBase
{
    [HttpGet("all")]
    public async Task<MessageData<PageData<PublishedPackageBasicDto>>> GetPackagePageAsync(
        string? keyword = null, int pageIndex = 1,
        int pageSize = 30, string? orderBy = null)
    {
        var packagePage =
            await packageService.GetPackagePageAsync(keyword, pageIndex, pageSize, orderBy);
        return SucceedPage(packagePage);
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

    [HttpGet("detail")]
    public async Task<MessageData<PublishedPackageDetailDto>> GetPackageDetailAsync(string id)
    {
        var package = await packageService.GetAsync(id);
        if (package is null)
        {
            throw new FriendlyException("节点包未找到", 1404);
        }

        return SucceedData(package.Adapt<PublishedPackageDetailDto>());
    }
}