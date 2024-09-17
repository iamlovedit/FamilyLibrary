using GalaFamilyLibrary.Model.FamilyLibrary;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyVersionService : IServiceBase<FamilyVersion>
    {
    }

    public class FamilyVersionService(IRepositoryBase<FamilyVersion> dbContext)
        : ServiceBase<FamilyVersion>(dbContext), IFamilyVersionService
    {
    }
}