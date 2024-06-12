using System.Globalization;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using GalaFamilyLibrary.DataTransferObject.FamilyLibrary;
using GalaFamilyLibrary.DataTransferObject.Identity;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Security;
using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Repository;
using GalaFamilyLibrary.Service.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.IdentityService.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("user/v{version:apiVersion}")]
    public class UserController(
        ITokenBuilder tokenBuilder,
        ILogger<UserController> logger,
        IRedisBasketRepository redis,
        IMapper mapper,
        IUserService userService,
        IUnitOfWork unitOfWork)
        : GalaControllerBase
    {
        [HttpGet("exist")]
        public async Task<MessageData<bool>> IsExistsAsync(string username)
        {
            var redisKey = $"users/username={username}";
            var user = default(User);
            if (await redis.Exist(redisKey))
            {
                user = await redis.Get<User>(redisKey);
                return Success(true);
            }
            else
            {
                user = await userService.GetFirstByExpressionAsync(u => u.Username == username);
                if (user is null)
                {
                    return Failed<bool>("user not exists");
                }

                await redis.Set(redisKey, user, TimeSpan.FromDays(7));
                return Success(true);
            }
        }

        [HttpGet("me")]
        public async Task<MessageData<UserDTO>> GetUserDetails()
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
            {
                return Failed<UserDTO>("obtain user id failed");
            }

            var user = await userService.GetByIdAsync(userId);

            return Success(mapper.Map<UserDTO>(user));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageData<TokenInfo>> RegisterAsync([FromBody] UserCreationDTO userCreationDTO,
            [FromServices] IRoleService roleService, [FromServices] IUserRoleService userRoleService)
        {
            var redisKey = $"user/register/username={userCreationDTO.Username}&password={userCreationDTO.Password}";
            if (await redis.Exist(redisKey))
            {
                return Failed<TokenInfo>("invalid request", 400);
            }

            await redis.Set(redisKey, userCreationDTO, TimeSpan.FromSeconds(1));

            var user = await userService.GetFirstByExpressionAsync(u => u.Username == userCreationDTO.Username);
            if (user != null)
            {
                return Failed<TokenInfo>("user already exists!", 300);
            }

            user = mapper.Map<User>(userCreationDTO);
            user.Password = userCreationDTO.Password!.MD5Encrypt32(user.Salt!);
            var role = await roleService.GetFirstByExpressionAsync(r => r.Name == PermissionConstants.ROLE_CONSUMER);
            if (role is null)
            {
                logger.LogWarning("role: {roleName} not exists", PermissionConstants.ROLE_CONSUMER);
                return Failed<TokenInfo>("register failed", 500);
            }

            try
            {
                unitOfWork.BeginTransaction();
                var userId = await userService.AddSnowflakeAsync(user);
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = role.Id
                };
                await userRoleService.AddSnowflakeAsync(userRole);
                unitOfWork.CommitTransaction();

                var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, user.Username!),
                    new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                    new(ClaimTypes.Expiration,
                        DateTime.Now.AddSeconds(tokenBuilder.GetTokenExpirationSeconds()).ToString()),
                    new(JwtRegisteredClaimNames.Iat,
                        EpochTime.GetIntDate(DateTime.Now).ToString(CultureInfo.InvariantCulture),
                        ClaimValueTypes.Integer64),
                    new(ClaimTypes.Role, role.Name!)
                };

                var token = tokenBuilder.GenerateTokenInfo(claims);
                return Success(token);
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                return Failed<TokenInfo>(e.Message, 500);
            }
        }
    }
}