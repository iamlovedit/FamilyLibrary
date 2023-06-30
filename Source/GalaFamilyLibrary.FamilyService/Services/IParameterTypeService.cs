using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services
{
    public interface IParameterTypeService : IServiceBase<ParameterType>
    {

    }

    public class ParameterTypeService : ServiceBase<ParameterType>, IParameterTypeService
    {
        public ParameterTypeService(IRepositoryBase<ParameterType> dbContext) : base(dbContext)
        {
        }
    }
}
