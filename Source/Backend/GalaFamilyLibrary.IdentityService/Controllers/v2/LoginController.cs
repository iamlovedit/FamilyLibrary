using Asp.Versioning;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Service.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.IdentityService.Controllers.v2;

[ApiVersion("2.0")]
[Route("/identity/v{version:apiVersion}/authenticate")]
public class LoginController(
    PermissionRequirement permissionRequirement,
    ILogger<LoginController> logger,
    IUserService userService)
    : GalaControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("V2");
    }
}