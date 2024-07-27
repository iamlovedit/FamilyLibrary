using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyCollectionsService : IServiceBase<FamilyCollections>
    {
    }

    public class FamilyCollectionsService(IRepositoryBase<FamilyCollections> dbContext)
        : ServiceBase<FamilyCollections>(dbContext), IFamilyCollectionsService
    {
    }
}