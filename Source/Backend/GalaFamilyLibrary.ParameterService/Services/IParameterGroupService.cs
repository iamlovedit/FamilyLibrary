using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.ParameterService.Services
{
    public interface IParameterGroupService : IServiceBase<ParameterGroup>
    {

    }
    public class ParameterGroupService(IRepositoryBase<ParameterGroup> dbContext)
        : ServiceBase<ParameterGroup>(dbContext), IParameterGroupService;
}
