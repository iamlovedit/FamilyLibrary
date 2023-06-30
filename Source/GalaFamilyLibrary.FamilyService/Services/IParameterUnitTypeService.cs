using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services
{
    public interface IParameterUnitTypeService : IServiceBase<ParameterUnitType>
    {

    }
    public class ParameterUnitTypeService : ServiceBase<ParameterUnitType>, IParameterUnitTypeService
    {
        public ParameterUnitTypeService(IRepositoryBase<ParameterUnitType> dbContext) : base(dbContext)
        {
        }
    }
}
