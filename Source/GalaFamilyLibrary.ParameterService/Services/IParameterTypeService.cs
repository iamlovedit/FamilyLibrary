using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.ParameterService.Services
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
