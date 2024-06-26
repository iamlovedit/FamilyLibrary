using Asp.Versioning;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Service.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.IdentityService.Controllers.v2;

[ApiVersion("1.0")]
[Authorize]
[Route("/identity/v{version:apiVersion}")]
public class UserController(IUserService userService) : GalaControllerBase
{
    private readonly IUserService _userService = userService;
}