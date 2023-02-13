using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.UserService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.UserService.Controllers.v2;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/authenticate")]
public class LoginController : ApiControllerBase
{
    private readonly PermissionRequirement _permissionRequirement;
    private readonly ILogger<LoginController> _logger;
    private readonly IUserService _userService;

    public LoginController(PermissionRequirement permissionRequirement, ILogger<LoginController> logger,
        IUserService userService)
    {
        _permissionRequirement = permissionRequirement;
        _logger = logger;
        _userService = userService;
    }
}