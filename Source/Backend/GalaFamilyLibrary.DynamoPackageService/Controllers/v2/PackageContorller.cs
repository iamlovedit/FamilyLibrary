using Asp.Versioning;
using GalaFamilyLibrary.Infrastructure;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Service.Package;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v2;

[ApiVersion("2.0")]
[Route("package/v{version:apiVersion}")]
public class PackageController(IPublishedPackageService packageService, ILogger<PackageController> logger)
    : DefaultControllerBase
{
    
}