using GalaFamilyLibrary.Model.FamilyParameter;

namespace GalaFamilyLibrary.Service.FamilyParameter;

public interface IParameterDefinitionService : IServiceBase<ParameterDefinition, long>
{
}

public class ParameterDefinitionService(IRepositoryBase<ParameterDefinition, long> dbContext)
    : ServiceBase<ParameterDefinition, long>(dbContext), IParameterDefinitionService
{
}