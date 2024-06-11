using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.ParameterService.Services
{
    public interface IParameterUnitTypeService : IServiceBase<ParameterUnitType>
    {

    }
    public class ParameterUnitTypeService(IRepositoryBase<ParameterUnitType> dbContext)
        : ServiceBase<ParameterUnitType>(dbContext), IParameterUnitTypeService;
}
