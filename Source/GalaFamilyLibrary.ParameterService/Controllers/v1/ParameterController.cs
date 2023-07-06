using AutoMapper;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyParameter;
using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.ParameterService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.ParameterService.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("parameter/v{version:apiVersion}")]
    public class ParameterController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ParameterController> _logger;
        private readonly IParameterDefinitionService _parameterDefinitionService;
        private readonly IRedisBasketRepository _redis;
        private readonly RedisRequirement _redisRequirement;
        private readonly IParameterService _parameterService;

        public ParameterController(IMapper mapper, ILogger<ParameterController> logger,
            IParameterDefinitionService parameterDefinitionService,
            IRedisBasketRepository redis, RedisRequirement redisRequirement, IParameterService parameterService)
        {
            _mapper = mapper;
            _logger = logger;
            _parameterDefinitionService = parameterDefinitionService;
            _redis = redis;
            _redisRequirement = redisRequirement;
            _parameterService = parameterService;
        }

        [HttpGet]
        [Route("details/{id:long}")]
        public async Task<MessageModel<ParameterDTO>> GetParameterDetailsAsync(long id)
        {
            var redisKey = $"parameters/{id}";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<ParameterDTO>(redisKey));
            }

            var parameter = await _parameterService.GetParameterDetailsAsync(id);
            if (parameter is null)
            {
                _logger.LogWarning("query parameter details failed,id {id} not exists", id);
                return Failed<ParameterDTO>("Resource not exists", 404);
            }

            var parameterDTO = _mapper.Map<ParameterDTO>(parameter);
            await _redis.Set(redisKey, parameterDTO, _redisRequirement.CacheTime);
            return Success(parameterDTO);
        }

        [HttpPost]
        public async Task<MessageModel<bool>> CreateParametersAsync([FromBody] List<ParameterCreationDTO> creationDTOs)
        {
            var parameters = _mapper.Map<List<Parameter>>(creationDTOs);
            var ids = await _parameterService.AddSnowflakesAsync(parameters);
            if (ids.Count > 0)
            {
                _logger.LogInformation("create parameters succeed,count : {count}", ids.Count);
                return Success(true);
            }

            return Failed<bool>();
        }


        [HttpGet]
        [Route("definitions/{id:long}")]
        public async Task<MessageModel<ParameterDefinitionDTO>> GetDefinitionDetailsAsync(long id)
        {
            var redisKey = $"parameter/definitions/{id}";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<ParameterDefinitionDTO>(redisKey));
            }

            var defintion = await _parameterDefinitionService.GetDefinitionDetailsAsync(id);
            if (defintion is null)
            {
                return Failed<ParameterDefinitionDTO>("definition not exists", 404);
            }

            var definitionDTO = _mapper.Map<ParameterDefinitionDTO>(defintion);
            await _redis.Set(redisKey, definitionDTO, _redisRequirement.CacheTime);
            return Success(definitionDTO);
        }

        [HttpPost]
        [Route("definition")]
        public async Task<MessageModel<bool>> CreateDefinitionsAsync(
            [FromBody] List<ParameterDefinitionCreationDTO> creationDTOs)
        {
            var definitions = _mapper.Map<List<ParameterDefinition>>(creationDTOs);
            var ids = await _parameterDefinitionService.AddSnowflakesAsync(definitions);
            if (ids.Count > 0)
            {
                _logger.LogInformation("create parameter definitions succeed,count : {count}", ids.Count);
                return Success(true);
            }

            return Failed<bool>();
        }


        [HttpGet]
        [Route("groups")]
        public async Task<MessageModel<List<ParameterGroupDTO>>> GetParameterGroupsAsync(
            [FromServices] IParameterGroupService groupService)
        {
            var redisKey = $"parameter/groups";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<List<ParameterGroupDTO>>(redisKey));
            }

            _logger.LogInformation("query all parameter groups");
            var groups = await groupService.GetAllAsync();
            var groupDTOs = _mapper.Map<List<ParameterGroupDTO>>(groups);
            await _redis.Set(redisKey, groupDTOs, TimeSpan.FromDays(7));
            return Success(groupDTOs);
        }

        [HttpGet]
        [Route("types")]
        public async Task<MessageModel<List<ParameterTypeDTO>>> GetParameterTypesAsync(
            [FromServices] IParameterTypeService typeService)
        {
            var redisKey = $"parameter/types";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<List<ParameterTypeDTO>>(redisKey));
            }

            _logger.LogInformation("query all parameter types");
            var types = await typeService.GetAllAsync();
            var typeDTOs = _mapper.Map<List<ParameterTypeDTO>>(types);
            await _redis.Set(redisKey, typeDTOs, TimeSpan.FromDays(7));
            return Success(typeDTOs);
        }

        [HttpGet]
        [Route("unitTypes")]
        public async Task<MessageModel<List<UnitTypeDTO>>> GetUnitTypesAsync(
            [FromServices] IParameterUnitTypeService unitTypeService)
        {
            var redisKey = $"parameter/unitTypes";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<List<UnitTypeDTO>>(redisKey));
            }

            _logger.LogInformation("query all parameter unit types");
            var unitTypes = await unitTypeService.GetAllAsync();
            var unitTypeDTOs = _mapper.Map<List<UnitTypeDTO>>(unitTypes);
            await _redis.Set(redisKey, unitTypeDTOs, TimeSpan.FromDays(7));
            return Success(unitTypeDTOs);
        }
    }
}