using GalaFamilyLibrary.Model.FamilyLibrary;


namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyVersionService : IServiceBase<FamilyVersion, long>
    {
    }

    public class FamilyVersionService(IRepositoryBase<FamilyVersion, long> dbContext)
        : ServiceBase<FamilyVersion, long>(dbContext), IFamilyVersionService
    {
    }
}