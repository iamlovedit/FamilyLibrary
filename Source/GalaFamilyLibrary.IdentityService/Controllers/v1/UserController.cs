using AutoMapper;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary;
using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;
using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Domain.Models.Identity;
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
    [Route("user/v{version:apiVersion}")]
    public class UserController : ApiControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFamilyCollectionService _familyCollectionService;
        private readonly IFamilyStarService _familyStarService;
        private readonly IRedisBasketRepository _redis;
        private readonly RedisRequirement _requirement;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper,
            IFamilyCollectionService familyCollectionService, IFamilyStarService familyStarService,
            IRedisBasketRepository redis, RedisRequirement requirement)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _familyCollectionService = familyCollectionService;
            _familyStarService = familyStarService;
            _redis = redis;
            _requirement = requirement;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<MessageModel<string>> CreateUser([FromBody] UserCreationDTO userCreationDto)
        {
            if (await _userService.GetFirstByExpressionAsync(u => u.Username == userCreationDto.Username) != null)
            {
                return Failed("用户名已存在");
            }

            var user = _mapper.Map<ApplicationUser>(userCreationDto);
            var id = await _userService.AddSnowflakeAsync(user);
            if (id > 0)
            {
                return Success("注册成功");
            }

            return Failed("注册失败");
        }


        [HttpGet("{id}")]
        public async Task<MessageModel<ApplicationUserDTO>> Details(int id)
        {
            var redisKey = $"user/{id}";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<ApplicationUserDTO>(redisKey));
            }

            var user = await _userService.GetByIdAsync(id);
            if (user is null)
            {
                return Failed<ApplicationUserDTO>("user not found");
            }

            var userDto = _mapper.Map<ApplicationUserDTO>(user);
            await _redis.Set(redisKey, userDto, _requirement.CacheTime);
            return Success(userDto);
        }


        [HttpGet]
        [Route("collections/{id:long}")]
        [AllowAnonymous]
        public async Task<MessageModel<PageModel<FamilyBasicDTO>>> GetUserCollection([FromRoute]long id, int pageIndex, int pageSize, string? orderField)
        {
            var redisKey = $"user/collections/{id}";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<PageModel<FamilyBasicDTO>>(redisKey));
            }

            var familiyPage = await _familyCollectionService.GetFamilyPageAsync(id, pageIndex, pageSize, orderField);
            var familyDTO = familiyPage.ConvertTo<FamilyBasicDTO>(_mapper);
            await _redis.Set(redisKey, familyDTO, _requirement.CacheTime);
            return Success(familyDTO);
        }
    }
}