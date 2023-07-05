using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.ParameterService.Services
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
