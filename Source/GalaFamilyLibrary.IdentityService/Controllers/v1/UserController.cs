using AutoMapper;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary;
using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;
using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.IdentityService.Models;
using GalaFamilyLibrary.IdentityService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Transaction;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisBasketRepository _redis;
        private readonly RedisRequirement _requirement;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper,
            IFamilyCollectionService familyCollectionService, IFamilyStarService familyStarService, IUnitOfWork unitOfWork,
            IRedisBasketRepository redis, RedisRequirement requirement)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _familyCollectionService = familyCollectionService;
            _familyStarService = familyStarService;
            _unitOfWork = unitOfWork;
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
        public async Task<MessageModel<PageModel<FamilyBasicDTO>>> GetUserCollection([FromRoute] long id, int pageIndex, int pageSize, string? orderField)
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

        [HttpDelete]
        [Route("collections")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> RemoveCollection([FromBody] CollectionCreationDTO collectionDTO)
        {
            var collection = await _familyCollectionService.
                GetFirstByExpressionAsync(c => c.FamilyId == collectionDTO.FamilyId && c.UserId == collectionDTO.UserId);
            if (collection is null)
            {
                return Failed<bool>("collection not exist", 404);
            }
            var user = await _userService.GetByIdAsync(collectionDTO.UserId);
            if (user is null)
            {
                return Failed<bool>("user not exist", 404);
            }
            try
            {
                _unitOfWork.BeginTransaction();
                collection.IsDeleted = true;
                var result = await _familyCollectionService.UpdateColumnsAsync(collection, c => c.IsDeleted);

                _unitOfWork.CommitTransaction();
                return result ? Success(true) : Failed<bool>();
            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        [HttpPost]
        [Route("collections")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> CreateCollection([FromBody] CollectionCreationDTO creationDTO)
        {
            var collection = _mapper.Map<FamilyCollection>(creationDTO);
            var id = await _familyCollectionService.AddAsync(collection);
            return id > 0 ? Success(true) : Failed<bool>();
        }

        [HttpDelete]
        [Route("stars")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> RemoveStar([FromBody] StarCreationDTO starDTO)
        {
            var star = await _familyStarService.
               GetFirstByExpressionAsync(c => c.FamilyId == starDTO.FamilyId && c.UserId == starDTO.UserId);
            if (star is null)
            {
                return Failed<bool>("star not exist", 404);
            }
            star.IsDeleted = true;

            var result = await _familyStarService.UpdateColumnsAsync(star, s => s.IsDeleted);
            return result ? Success(true) : Failed<bool>();
        }

        [HttpPost]
        [Route("stars")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> CreateStar([FromBody] StarCreationDTO starCreationDTO)
        {
            var star = _mapper.Map<FamilyStar>(starCreationDTO);
            var id = await _familyStarService.AddAsync(star);
            return id > 0 ? Success(true) : Failed<bool>();
        }
    }
}