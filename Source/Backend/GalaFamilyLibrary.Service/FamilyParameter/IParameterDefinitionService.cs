using GalaFamilyLibrary.Model.FamilyParameter;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.FamilyParameter;

public interface IParameterDefinitionService:IServiceBase<ParameterDefinition>
{
    
}

public class ParameterDefinitionService : ServiceBase<ParameterDefinition>, IParameterDefinitionService
{
    public ParameterDefinitionService(IRepositoryBase<ParameterDefinition> dbContext) : base(dbContext)
    {
    }
}