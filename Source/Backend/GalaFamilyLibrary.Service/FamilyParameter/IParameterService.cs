using GalaFamilyLibrary.Model.FamilyParameter;
namespace GalaFamilyLibrary.Service.FamilyParameter;

public interface IParameterService : IServiceBase<Parameter, long>
{
}

public class ParameterService(IRepositoryBase<Parameter, long> dbContext)
    : ServiceBase<Parameter, long>(dbContext), IParameterService
{
    
}