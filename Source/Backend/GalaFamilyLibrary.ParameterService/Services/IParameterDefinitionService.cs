using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.ParameterService.Services
{
    public interface IParameterDefinitionService : IServiceBase<ParameterDefinition>
    {
        Task<ParameterDefinition> GetDefinitionDetailsAsync(long id);
    }

    public class ParameterDefinitionService : ServiceBase<ParameterDefinition>, IParameterDefinitionService
    {
        public ParameterDefinitionService(IRepositoryBase<ParameterDefinition> dbContext) : base(dbContext)
        {
        }

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
