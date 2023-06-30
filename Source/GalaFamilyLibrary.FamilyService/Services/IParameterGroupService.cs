using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services
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
