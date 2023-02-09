using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.UserService.DataTransferObjects;
using GalaFamilyLibrary.UserService.Helpers;
using GalaFamilyLibrary.UserService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.UserService.Models;

namespace GalaFamilyLibrary.UserService.Controllers.v1
{
    [ApiVersion("1.0")]
    public class LoginController : ApiControllerBase
    {
        private readonly PermissionRequirement _permissionRequirement;
        private readonly ILogger<LoginController> _logger;
        private readonly IRedisBasketRepository _redis;
        private readonly IUserService _userService;

        public LoginController(PermissionRequirement permissionRequirement, ILogger<LoginController> logger,
            IRedisBasketRepository redis,
            IUserService userService)
        {
            _permissionRequirement = permissionRequirement;
            _logger = logger;
            _redis = redis;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<MessageModel<string>> GetTokenAsync([FromBody] LoginUserDto loginUser)
        {
            if (string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
            {
                _logger.LogWarning("");
                return Failed<string>("用户名或者密码不能为空");
            }

            var userKey = $"auth/user/{loginUser.Username}";
            var user = default(LibraryUser);
            if (await _redis.Exist(userKey))
            {
                user = await _redis.Get<LibraryUser>(userKey);
            }
            else
            {
                user = await _userService.GetFirstByExpressionAsync(u => u.Username == loginUser.Username);
                {
                    var password = loginUser.Password.MD5Encrypt32(user.Salt);
                    if (!password.Equals(user.Password))
                    {
                        return Failed<string>("用户名或者密码错误");
                    }

                    var tokenKey = $"auth/token/{user.Id}";
                    if (await _redis.Exist(tokenKey))
                    {
                        return Success(await _redis.Get<string>(tokenKey));
                    }
                    else
                    {
                        var roleKey = $"auth/roles/{user.Id}";
                        var roleNames = default(List<string>);
                        if (await _redis.Exist(roleKey))
                        {
                            roleNames = await _redis.Get<List<string>>(roleKey);
                        }
                        else
                        {
                            roleNames = await _userService.GetUserRolesByIdAsync(user.Id);
                        }

                        var claims = new List<Claim>()
                        {
                            new(ClaimTypes.Name, loginUser.Username),
                            new(ClaimTypes.Email, user.Email),
                            new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                            new(ClaimTypes.Expiration,
                                DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString())
                        };
                        user.LastLoginTime = DateTime.Now;
                        claims.AddRange(roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName)));
                        await _userService.UpdateAsync(user);
                        var token = GenerateToken(claims, _permissionRequirement);
                        await _redis.Set(tokenKey, token, _permissionRequirement.Expiration);
                        return Success(token);
                    }
                }
            }

            return Failed<string>("用户名或者密码错误");
        }

        public async Task<MessageModel<string>> RefreshTokenAsync()
        {
            throw new NotImplementedException();
        }

        private static string GenerateToken(List<Claim> claims, PermissionRequirement permissionRequirement)
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