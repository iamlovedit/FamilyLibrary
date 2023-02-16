using AutoMapper;
using GalaFamilyLibrary.IdentityService.DataTransferObjects;
using GalaFamilyLibrary.IdentityService.Models;
using GalaFamilyLibrary.IdentityService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.IdentityService.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Policy = "ElevatedRights")]
    [Route("v{version:apiVersion}")]
    public class UserController : ApiControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRedisBasketRepository _redis;
        private readonly RedisRequirement _requirement;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper,
            IRedisBasketRepository redis, RedisRequirement requirement)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _redis = redis;
            _requirement = requirement;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Regsiter([FromBody] LibraryUserCreationDto userCreationDto)
        {
            if (await _userService.GetFirstByExpressionAsync(u => u.Username == userCreationDto.Username) != null)
            {
                return Failed("用户名已存在");
            }

            var user = _mapper.Map<LibraryUser>(userCreationDto);
            var id = await _userService.AddAsync(user);
            if (id > 0)
            {
                return Success("注册成功");
            }

            return Failed("注册失败");
        }

        [HttpGet("{id}")]
        public async Task<MessageModel<LibraryUserDto>> Details(int id)
        {
            var redisKey = $"user/{id}";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<LibraryUserDto>(redisKey));
            }

            var user = await _userService.GetByIdAsync(id);
            if (user is null)
            {
                return Failed<LibraryUserDto>("user not found");
            }

            var userDto = _mapper.Map<LibraryUserDto>(user);
            await _redis.Set(redisKey, userDto, _requirement.CacheTime);
            return Success(userDto);
        }
    }
}