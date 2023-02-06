using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.UserService.DataTransferObjetcts;
using GalaFamilyLibrary.UserService.Helpers;
using GalaFamilyLibrary.UserService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GalaFamilyLibrary.UserService.Controllers.v1
{

    [ApiVersion("1.0")]
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<MessageModel<string>> GetTokenAsync([FromBody] LoginUserDto loginUser)
        {
            if (string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
            {
                return Failed<string>("用户名或者密码不能为空");
            }
            var user = await _userService.GetFirstByExpressionAsync(u => u.Username == loginUser.Username);
            if (user != null)
            {
                var password = loginUser.Password.MD5Encrypt32(user.Salt);
                if (password.Equals(user.Password))
                {
                    var roleNames = await _userService.GetUserRolesByIdAsync(user.Id);
                    var claims = new List<Claim>()
                    {
                    new(ClaimTypes.Name, loginUser.Username),
                    new(ClaimTypes.Email, user.Email),
                    new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                    new(ClaimTypes.Expiration,
                        DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString())
                    };

                    user.LastLoginTime = DateTime.Now;
                    await _userService.UpdateAsync(user);
                    claims.AddRange(roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName)));
                    var token = GenerateToken(claims, _permissionRequirement);
                    return Success(token);
                }
                else
                {
                    return Failed<string>("用户名或者密码错误");
                }
            }
            else
            {
                return Failed<string>("用户名或者密码错误");
            }
        }

        private static string GenerateToken(IList<Claim> claims, PermissionRequirement permissionRequirement)
        {
            var now = DateTime.Now;
            var jwtToken = new JwtSecurityToken(
                issuer: permissionRequirement.Issuer,
                audience: permissionRequirement.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(permissionRequirement.Expiration),
                signingCredentials: permissionRequirement.SigningCredentials
            );
            var value = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local),
                tokenType = JwtBearerDefaults.AuthenticationScheme
            };
            return JsonConvert.SerializeObject(value);
        }
    }
}
