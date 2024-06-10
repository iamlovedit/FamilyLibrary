using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v2;

[ApiVersion("2.0")]
[Route("package/v{version:apiVersion}")]
public class PackageController(IPackageService packageService, ILogger<PackageController> logger)
    : GalaControllerBase
{
    private readonly IPackageService _packageService = packageService;
    private readonly ILogger<PackageController> _logger = logger;

    [HttpGet]
    public async Task<string> Get()
    {
        return await Task.FromResult(nameof(v1.PackageController));
    }
}