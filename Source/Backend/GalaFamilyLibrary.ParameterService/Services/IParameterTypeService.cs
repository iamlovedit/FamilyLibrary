using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.ParameterService.Services
{
    public interface IParameterTypeService : IServiceBase<ParameterType>
    {

    }

    public class ParameterTypeService(IRepositoryBase<ParameterType> dbContext)
        : ServiceBase<ParameterType>(dbContext), IParameterTypeService;
}
