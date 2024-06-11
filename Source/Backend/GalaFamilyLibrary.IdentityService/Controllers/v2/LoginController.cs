using Asp.Versioning;
using GalaFamilyLibrary.IdentityService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.IdentityService.Controllers.v2;

[ApiVersion("1.0")]
[Route("/identity/v{version:apiVersion}/authenticate")]
public class LoginController(
    PermissionRequirement permissionRequirement,
    ILogger<LoginController> logger,
    IUserService userService)
    : GalaControllerBase
{
    private readonly PermissionRequirement _permissionRequirement = permissionRequirement;
    private readonly ILogger<LoginController> _logger = logger;
    private readonly IUserService _userService = userService;
}