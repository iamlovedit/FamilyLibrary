using AutoMapper;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Mvc;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.FamilyService.DataTransferObjects;
using GalaFamilyLibrary.FamilyService.Models;

namespace GalaFamilyLibrary.FamilyService.Controllers.v1;

[ApiVersion("1.0")]
[Route("family/parameter/v{version:apiVersion}")]
public class ParameterController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<ParameterController> _logger;
    private readonly IParameterDefinitionService _parameterDefinitionService;
    private readonly IRedisBasketRepository _redis;
    private readonly RedisRequirement _redisRequirement;
    private readonly IFamilyParameterService _parameterService;

    public ParameterController(IMapper mapper, ILogger<ParameterController> logger, IParameterDefinitionService parameterDefinitionService,
        IRedisBasketRepository redis, RedisRequirement redisRequirement, IFamilyParameterService parameterService)
    {
        _mapper = mapper;
        _logger = logger;
        _parameterDefinitionService = parameterDefinitionService;
        _redis = redis;
        _redisRequirement = redisRequirement;
        _parameterService = parameterService;
    }

    [HttpGet]
    [Route("parameters/{id:long}")]
    public async Task<MessageModel<FamilyParameterDTO>> GetParameterDetailsAysnc(long id)
    {
        var redisKey = $"parameters/{id}";
        if (await _redis.Exist(redisKey))
        {
            return Success(await _redis.Get<FamilyParameterDTO>(redisKey));
        }

        var parameter = _parameterService.GetParameterDetailsAsync(id);
        if (parameter is null)
        {
            _logger.LogWarning("query parameter details failed,id {id} not exists", id);
            return Failed<FamilyParameterDTO>("Resource not exists", 404);
        }

        var parameterDTO = _mapper.Map<FamilyParameterDTO>(parameter);
        await _redis.Set(redisKey, parameterDTO, _redisRequirement.CacheTime);
        return Success(parameterDTO);
    }

    [HttpPost]
    [Route("parameters")]
    public async Task<MessageModel<bool>> CreateParameters(List<ParameterCreationDTO> creationDTOs)
    {
        var parameters = _mapper.Map<List<FamilyParameter>>(creationDTOs);
        var ids = await _parameterService.AddSnowflakesAsync(parameters);
        if (ids.Count > 0)
        {
            return Success(true);
        }
        return Failed<bool>();
    }



    [HttpGet]
    [Route("definitions/{id:long}")]
    public async Task<MessageModel<ParameterDefinitionDTO>> GetDefinitionDetails(long id)
    {
        var redisKey = $"parameter/definitions/{id}";
        if (await _redis.Exist(redisKey))
        {
            return Success(await _redis.Get<ParameterDefinitionDTO>(redisKey));
        }
        var defintion = _parameterDefinitionService.GetDefinitionDetailsAsync(id);
        if (defintion is null)
        {
            return Failed<ParameterDefinitionDTO>("definition not exists", 404);
        }
        var definitionDTO = _mapper.Map<ParameterDefinitionDTO>(defintion);
        await _redis.Set(redisKey, definitionDTO, _redisRequirement.CacheTime);
        return Success(definitionDTO);
    }




    [HttpGet]
    [Route("groups")]
    public async Task<MessageModel<List<ParameterGroupDTO>>> GetAllParameterGroups(
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
    public async Task<MessageModel<List<ParameterTypeDTO>>> GetAllParameterTypes(
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
    public async Task<MessageModel<List<UnitTypeDTO>>> GetAllUnitTypes(
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