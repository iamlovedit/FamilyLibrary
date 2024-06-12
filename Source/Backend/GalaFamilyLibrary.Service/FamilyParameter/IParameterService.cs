using GalaFamilyLibrary.Model.FamilyParameter;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.FamilyParameter;

public interface IParameterService:IServiceBase<Parameter>
{
    
}

public class ParameterService : ServiceBase<Parameter>, IParameterService
{
    public ParameterService(IRepositoryBase<Parameter> dbContext) : base(dbContext)
    {
    }
}