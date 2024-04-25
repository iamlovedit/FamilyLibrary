using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.ParameterService.Services
{
    public interface IParameterGroupService : IServiceBase<ParameterGroup>
    {

    }
    public class ParameterGroupService : ServiceBase<ParameterGroup>, IParameterGroupService
    {
        public ParameterGroupService(IRepositoryBase<ParameterGroup> dbContext) : base(dbContext)
        {

        }
    }
}
