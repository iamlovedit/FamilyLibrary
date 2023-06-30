using AutoMapper;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Mvc;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.FamilyService.DataTransferObjects;

namespace GalaFamilyLibrary.FamilyService.Controllers.v1;

[ApiVersion("1.0")]
[Route("family/parameter/v{version:apiVersion}")]
public class ParameterController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<ParameterController> _logger;
    private readonly IRedisBasketRepository _redis;
    private readonly RedisRequirement _redisRequirement;
    private readonly IFamilyParameterService _parameterService;

    public ParameterController(IMapper mapper, ILogger<ParameterController> logger,
        IRedisBasketRepository redis, RedisRequirement redisRequirement, IFamilyParameterService parameterService)
    {
        _mapper = mapper;
        _logger = logger;
        _redis = redis;
        _redisRequirement = redisRequirement;
        _parameterService = parameterService;
    }

    [HttpGet]
    [Route("details/{id:long}")]
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

    [HttpGet]
    [Route("groups")]
    public async Task<MessageModel<List<ParameterGroupDTO>>> GetAllParameterGroups([FromServices] IParameterGroupService groupService)
    {
        var redisKey = $"parameter/groups";

        var groups = await groupService.GetAllAsync();
        var groupDTOs = _mapper.Map<List<ParameterGroupDTO>>(groups);
        return Success(groupDTOs);
    }

    [HttpGet]
    [Route("types")]
    public async Task<MessageModel<List<ParameterTypeDTO>>> GetAllParameterTypes([FromServices] IParameterTypeService typeService)
    {
        var redisKey = $"parameter/types";
        var types = typeService.GetAllAsync();
        var typeDTOs = _mapper.Map<List<ParameterTypeDTO>>(types);
        return Success(typeDTOs);
    }

    [HttpGet]
    [Route("types")]
    public async Task<MessageModel<List<UnitTypeDTO>>> GetAllUnitTypes([FromServices] IParameterUnitTypeService unitTypeService)
    {
        var redisKey = $"parameter/groups";

        var unitTypes = unitTypeService.GetAllAsync();
        var unitTypeDTOs = _mapper.Map<List<UnitTypeDTO>>(unitTypes);
        return Success(unitTypeDTOs);
    }
}

