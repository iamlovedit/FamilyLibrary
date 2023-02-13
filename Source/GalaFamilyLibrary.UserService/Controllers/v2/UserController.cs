using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.UserService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.UserService.Controllers.v2;

[ApiVersion("1.0")]
[Authorize]
[Route("v{version:apiVersion}")]
public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
}