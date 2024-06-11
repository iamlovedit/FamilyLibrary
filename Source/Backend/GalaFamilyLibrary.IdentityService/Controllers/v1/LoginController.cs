using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;
using GalaFamilyLibrary.IdentityService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Security;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using GalaFamilyLibrary.IdentityService.Services;
using GalaFamilyLibrary.Infrastructure.Redis;

namespace GalaFamilyLibrary.IdentityService.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("identity/v{version:apiVersion}/authenticate")]
    public class LoginController(
        PermissionRequirement permissionRequirement,
        ILogger<LoginController> logger,
        IRedisBasketRepository redis,
        IAESEncryptionService aesEncryptionService,
        ITokenBuilder tokenBuilder,
        IUserService userService)
        : GalaControllerBase
    {
        private readonly IRedisBasketRepository _redis = redis;
        private readonly IAESEncryptionService _aesEncryptionService = aesEncryptionService;

        [HttpGet]
        [Authorize(Roles = PermissionConstants.ROLE_ADMINISTRATOR)]
        public IActionResult Test()
        {
            return Ok("HelloWorld");
        }


        /// <summary>
        /// v1/authenticate/token
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<MessageData<TokenInfo?>> GetTokenAsync([FromBody] UserLoginDTO loginUser)
        {
            if (string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
            {
                return Failed<TokenInfo>("用户名或者密码不能为空");
            }

            var user = await userService.GetFirstByExpressionAsync(u => u.Username == loginUser.Username);
            if (user != null)
            {
                var password = loginUser.Password.MD5Encrypt32(user.Salt);
                if (!password.Equals(user.Password))
                {
                    user.ErrorCount += 1;
                    await userService.UpdateColumnsAsync(user, u => u.ErrorCount);
                    return Failed<TokenInfo>("用户名或者密码错误");
                }

                logger.LogInformation("user {user} logged in", user.Username);
                user.ErrorCount = 0;
                user.LastLoginTime = DateTime.Now;
                await userService.UpdateColumnsAsync(user, u => new { u.ErrorCount, u.LastLoginTime });

                var roleNames = await userService.GetUserRolesByIdAsync(user.Id);
                var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, loginUser.Username),
                    new(ClaimTypes.Email, user.Email),
                    new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                    new(ClaimTypes.Expiration,
                        DateTime.Now.AddSeconds(permissionRequirement.Expiration.TotalSeconds).ToString())
                };
                claims.AddRange(roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName)));
                var token = tokenBuilder.GenerateTokenInfo(claims);
                return Success(token);
            }
            return Failed<TokenInfo>("用户名或者密码错误");
        }

        /// <summary>
        /// v1/authenticate/logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public Task<MessageData<bool>> LogoutAsync()
        {
            return Task.FromResult(Success(true));
        }

        /// <summary>
        /// v1/authenticate/refresh
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<MessageData<TokenInfo>> RefreshTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Failed<TokenInfo>("token is invalid");
            }

            var jwtHandler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = jwtHandler.ReadJwtToken(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            throw new NotImplementedException();
        }
    }
}