using Asp.Versioning;
using GalaFamilyLibrary.DataTransferObject.Package;
using GalaFamilyLibrary.Infrastructure;
using GalaFamilyLibrary.Service.Package;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v2;

[ApiVersion("2.0")]
[Route("package/v{version:apiVersion}")]
public class PackageController(IPublishedPackageService packageService, ILogger<PackageController> logger)
    : DefaultControllerBase
{
    [HttpGet("all")]
    public async Task<MessageData<PageData<PublishedPackageDto>>> GetPackagePageAsync(
        string? keyword = null, int pageIndex = 1,
        int pageSize = 30, string? orderBy = null)
    {
        var packagePage =
            await packageService.GetPackagePageAsync(keyword, pageIndex, pageSize, orderBy);
        return SucceedPage(packagePage);
    }
}