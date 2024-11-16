using System.Globalization;
using System.Security.Claims;
using Asp.Versioning;
using GalaFamilyLibrary.DataTransferObject.Identity;
using GalaFamilyLibrary.Infrastructure;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Security;
using GalaFamilyLibrary.Service.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;


namespace GalaFamilyLibrary.IdentityService.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("identity/v{version:apiVersion}/auth")]
    public class LoginController(
        ILogger<LoginController> logger,
        IRedisBasketRepository redis,
        ITokenBuilder tokenBuilder,
        IUserService userService)
        : DefaultControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<MessageData<TokenInfo>> LoginAsync([FromBody] UserLoginDTO loginUser)
        {
            if (string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
            {
                return Fail<TokenInfo>("password or username is empty");
            }

            var lockKey = $"auth/username={loginUser.Username}&password={loginUser.Password}";
            if (await redis.Exist(lockKey))
            {
                return Fail<TokenInfo>("invalid request", 400);
            }

            await redis.Set(lockKey, loginUser, TimeSpan.FromSeconds(1));
            var user = await userService.GetFirstByExpressionAsync(u => u.Username == loginUser.Username);
            if (user is null)
            {
                return Fail<TokenInfo>("username or password is incorrect");
            }

            var password = loginUser.Password!.MD5Encrypt32(user.Salt!);
            if (password != user.Password)
            {
                return Fail<TokenInfo>("username or password is incorrect");
            }

            var roles = await userService.GetUserRolesAsync(user.Id);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.Username!),
                new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                new(JwtRegisteredClaimNames.Iat,
                    EpochTime.GetIntDate(DateTime.Now).ToString(CultureInfo.InvariantCulture),
                    ClaimValueTypes.Integer64),
                new(ClaimTypes.Expiration,
                    DateTime.Now.AddSeconds(tokenBuilder.GetTokenExpirationSeconds()).ToString())
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Name!)));
            var token = tokenBuilder.GenerateTokenInfo(claims);
            return Succeed(token);
        }

        [HttpPost("refresh")]
        public async Task<MessageData<TokenInfo>> RefreshTokenAsync([FromForm] string token)
        {
            var lockKey = $"auth/token/refresh/token={token}";
            if (await redis.Exist(lockKey))
            {
                return Fail<TokenInfo>("invalid request");
            }

            await redis.Set(lockKey, token, TimeSpan.FromSeconds(1));

            if (string.IsNullOrEmpty(token))
            {
                return Fail<TokenInfo>("token is invalid");
            }

            token = tokenBuilder.DecryptCipherToken(token);
            var uid = tokenBuilder.ParseUIdFromToken(token);
            if (!tokenBuilder.VerifyToken(token) || uid <= 0)
            {
                return Fail<TokenInfo>("refresh failed");
            }

            var user = await userService.GetByIdAsync(uid);
            if (user is null)
            {
                return Fail<TokenInfo>("refresh failed");
            }

            var roles = await userService.GetUserRolesAsync(uid);
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.Username!),
                new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                new(JwtRegisteredClaimNames.Iat,
                    EpochTime.GetIntDate(DateTime.Now).ToString(CultureInfo.InvariantCulture),
                    ClaimValueTypes.Integer64),
                new(ClaimTypes.Expiration,
                    DateTime.Now.AddSeconds(tokenBuilder.GetTokenExpirationSeconds()).ToString())
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Name!)));
            var tokenInfo = tokenBuilder.GenerateTokenInfo(claims);
            return Succeed(tokenInfo);
        }
    }
}