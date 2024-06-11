using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.ParameterService.Services
{
    public interface IParameterTypeService : IServiceBase<ParameterType>
    {

    }

    public class ParameterTypeService(IRepositoryBase<ParameterType> dbContext)
        : ServiceBase<ParameterType>(dbContext), IParameterTypeService;
}
