using AutoMapper;
using FluentValidation;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary;
using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;
using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.IdentityService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.IdentityService.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("user/v{version:apiVersion}")]
    public class UserController(
        ILogger<UserController> logger,
        IUserService userService,
        IMapper mapper,
        IFamilyCollectionService familyCollectionService,
        IFamilyStarService familyStarService,
        IUnitOfWork unitOfWork,
        IValidator<UserCreationDTO> userCreationValidator,
        IRedisBasketRepository redis,
        RedisRequirement requirement)
        : ApiControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<MessageModel<string>> CreateUser([FromBody] UserCreationDTO userCreationDto)
        {
            if (await userService.GetFirstByExpressionAsync(u => u.Username == userCreationDto.Username) != null)
            {
                return Failed("用户名已存在");
            }

            var user = mapper.Map<User>(userCreationDto);
            var id = await userService.AddSnowflakeAsync(user);
            if (id > 0)
            {
                return Success("注册成功");
            }

            return Failed("注册失败");
        }


        [HttpGet("{id:long}")]
        public async Task<MessageModel<ApplicationUserDTO>> Details(long id)
        {
            var redisKey = $"user/{id}";
            if (await redis.Exist(redisKey))
            {
                return Success(await redis.Get<ApplicationUserDTO>(redisKey));
            }

            var user = await userService.GetByIdAsync(id);
            if (user is null)
            {
                return Failed<ApplicationUserDTO>("user not found");
            }

            var userDto = mapper.Map<ApplicationUserDTO>(user);
            await redis.Set(redisKey, userDto, requirement.CacheTime);
            return Success(userDto);
        }


        [HttpGet]
        [Route("collections/{id:long}")]
        [AllowAnonymous]
        public async Task<MessageModel<PageModel<FamilyBasicDTO>>> GetUserCollection([FromRoute] long id, int pageIndex, int pageSize, string? orderField)
        {
            var redisKey = $"user/collections/{id}";
            if (await redis.Exist(redisKey))
            {
                return Success(await redis.Get<PageModel<FamilyBasicDTO>>(redisKey));
            }

            var familiyPage = await familyCollectionService.GetFamilyPageAsync(id, pageIndex, pageSize, orderField);
            var familyDTO = familiyPage.ConvertTo<FamilyBasicDTO>(mapper);
            await redis.Set(redisKey, familyDTO, requirement.CacheTime);
            return Success(familyDTO);
        }

        [HttpDelete]
        [Route("collections")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> RemoveCollection([FromBody] CollectionCreationDTO collectionDTO)
        {
            var collection = await familyCollectionService.
                GetFirstByExpressionAsync(c => c.FamilyId == collectionDTO.FamilyId && c.UserId == collectionDTO.UserId);
            if (collection is null)
            {
                return Failed<bool>("collection not exist", 404);
            }
            var user = await userService.GetByIdAsync(collectionDTO.UserId);
            if (user is null)
            {
                return Failed<bool>("user not exist", 404);
            }
            try
            {
                unitOfWork.BeginTransaction();
                collection.IsDeleted = true;
                var result = await familyCollectionService.UpdateColumnsAsync(collection, c => c.IsDeleted);

                unitOfWork.CommitTransaction();
                return result ? Success(true) : Failed<bool>();
            }
            catch (Exception e)
            {
                unitOfWork.RollbackTransaction();
                throw;
            }
        }

        [HttpPost]
        [Route("collections")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> CreateCollection([FromBody] CollectionCreationDTO creationDTO)
        {
            var collection = mapper.Map<FamilyCollection>(creationDTO);
            var id = await familyCollectionService.AddAsync(collection);
            return id > 0 ? Success(true) : Failed<bool>();
        }

        [HttpDelete]
        [Route("stars")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> RemoveStar([FromBody] StarCreationDTO starDTO)
        {
            var star = await familyStarService.
               GetFirstByExpressionAsync(c => c.FamilyId == starDTO.FamilyId && c.UserId == starDTO.UserId);
            if (star is null)
            {
                return Failed<bool>("star not exist", 404);
            }
            star.IsDeleted = true;

            var result = await familyStarService.UpdateColumnsAsync(star, s => s.IsDeleted);
            return result ? Success(true) : Failed<bool>();
        }

        [HttpPost]
        [Route("stars")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> CreateStar([FromBody] StarCreationDTO starCreationDTO)
        {
            var star = mapper.Map<FamilyStar>(starCreationDTO);
            var id = await familyStarService.AddAsync(star);
            return id > 0 ? Success(true) : Failed<bool>();
        }
    }
}