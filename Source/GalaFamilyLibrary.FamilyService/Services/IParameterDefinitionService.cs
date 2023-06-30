using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services
{
    public interface IParameterDefinitionService : IServiceBase<ParameterDefinition>
    {

    }

    public class ParameterDefinitionService : ServiceBase<ParameterDefinition>, IParameterDefinitionService
    {
        public ParameterDefinitionService(IRepositoryBase<ParameterDefinition> dbContext) : base(dbContext)
        {
        }
    }
}
