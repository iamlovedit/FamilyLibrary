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
    public class ParameterController(
        IMapper mapper,
        ILogger<ParameterController> logger,
        IParameterDefinitionService parameterDefinitionService,
        IRedisBasketRepository redis,
        RedisRequirement redisRequirement,
        IParameterService parameterService)
        : GalaControllerBase
    {
        [HttpGet]
        [Route("details/{id:long}")]
        public async Task<MessageData<ParameterDTO?>> GetParameterDetailsAsync(long id)
        {
            var redisKey = $"parameters/{id}";
            if (await redis.Exist(redisKey))
            {
                return Success(await redis.Get<ParameterDTO>(redisKey));
            }

            var parameter = await parameterService.GetParameterDetailsAsync(id);
            if (parameter is null)
            {
                logger.LogWarning("query parameter details failed,id {id} not exists", id);
                return Failed<ParameterDTO>("Resource not exists", 404);
            }

            var parameterDTO = mapper.Map<ParameterDTO>(parameter);
            await redis.Set(redisKey, parameterDTO, redisRequirement.CacheTime);
            return Success(parameterDTO);
        }

        [HttpPost]
        public async Task<MessageData<bool>> CreateParametersAsync([FromBody] List<ParameterCreationDTO> creationDTOs)
        {
            var parameters = mapper.Map<List<Parameter>>(creationDTOs);
            var ids = await parameterService.AddSnowflakesAsync(parameters);
            if (ids.Count > 0)
            {
                logger.LogInformation("create parameters succeed,count : {count}", ids.Count);
                return Success(true);
            }

            return Failed<bool>();
        }


        [HttpGet]
        [Route("definitions/{id:long}")]
        public async Task<MessageData<ParameterDefinitionDTO?>> GetDefinitionDetailsAsync(long id)
        {
            var redisKey = $"parameter/definitions/{id}";
            if (await redis.Exist(redisKey))
            {
                return Success(await redis.Get<ParameterDefinitionDTO>(redisKey));
            }

            var defintion = await parameterDefinitionService.GetDefinitionDetailsAsync(id);
            if (defintion is null)
            {
                return Failed<ParameterDefinitionDTO>("definition not exists", 404);
            }

            var definitionDTO = mapper.Map<ParameterDefinitionDTO>(defintion);
            await redis.Set(redisKey, definitionDTO, redisRequirement.CacheTime);
            return Success(definitionDTO);
        }

        [HttpPost]
        [Route("definition")]
        public async Task<MessageData<bool>> CreateDefinitionsAsync(
            [FromBody] List<ParameterDefinitionCreationDTO> creationDTOs)
        {
            var definitions = mapper.Map<List<ParameterDefinition>>(creationDTOs);
            var ids = await parameterDefinitionService.AddSnowflakesAsync(definitions);
            if (ids.Count > 0)
            {
                logger.LogInformation("create parameter definitions succeed,count : {count}", ids.Count);
                return Success(true);
            }

            return Failed<bool>();
        }


        [HttpGet]
        [Route("groups")]
        public async Task<MessageData<List<ParameterGroupDTO>?>> GetParameterGroupsAsync(
            [FromServices] IParameterGroupService groupService)
        {
            var redisKey = $"parameter/groups";
            if (await redis.Exist(redisKey))
            {
                return Success(await redis.Get<List<ParameterGroupDTO>>(redisKey));
            }

            logger.LogInformation("query all parameter groups");
            var groups = await groupService.GetAllAsync();
            var groupDTOs = mapper.Map<List<ParameterGroupDTO>>(groups);
            await redis.Set(redisKey, groupDTOs, TimeSpan.FromDays(7));
            return Success(groupDTOs);
        }

        [HttpGet]
        [Route("types")]
        public async Task<MessageData<List<ParameterTypeDTO>?>> GetParameterTypesAsync(
            [FromServices] IParameterTypeService typeService)
        {
            var redisKey = $"parameter/types";
            if (await redis.Exist(redisKey))
            {
                return Success(await redis.Get<List<ParameterTypeDTO>>(redisKey));
            }

            logger.LogInformation("query all parameter types");
            var types = await typeService.GetAllAsync();
            var typeDTOs = mapper.Map<List<ParameterTypeDTO>>(types);
            await redis.Set(redisKey, typeDTOs, TimeSpan.FromDays(7));
            return Success(typeDTOs);
        }

        [HttpGet]
        [Route("unitTypes")]
        public async Task<MessageData<List<UnitTypeDTO>?>> GetUnitTypesAsync(
            [FromServices] IParameterUnitTypeService unitTypeService)
        {
            var redisKey = $"parameter/unitTypes";
            if (await redis.Exist(redisKey))
            {
                return Success(await redis.Get<List<UnitTypeDTO>>(redisKey));
            }

            logger.LogInformation("query all parameter unit types");
            var unitTypes = await unitTypeService.GetAllAsync();
            var unitTypeDTOs = mapper.Map<List<UnitTypeDTO>>(unitTypes);
            await redis.Set(redisKey, unitTypeDTOs, TimeSpan.FromDays(7));
            return Success(unitTypeDTOs);
        }
    }
}