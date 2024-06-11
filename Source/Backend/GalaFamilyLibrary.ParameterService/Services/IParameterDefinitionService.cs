using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.ParameterService.Services
{
    public interface IParameterDefinitionService : IServiceBase<ParameterDefinition>
    {
        Task<ParameterDefinition> GetDefinitionDetailsAsync(long id);
    }

    public class ParameterDefinitionService(IRepositoryBase<ParameterDefinition> dbContext)
        : ServiceBase<ParameterDefinition>(dbContext), IParameterDefinitionService
    {
        public async Task<ParameterDefinition> GetDefinitionDetailsAsync(long id)
        {
            return await DAL.DbContext.Queryable<ParameterDefinition>()
                    .Includes(f => f.ParameterGroup)
                    .Includes(f => f.ParameterType)
                    .Includes(f => f.UnitType)
                    .InSingleAsync(id);
        }
    }
}
