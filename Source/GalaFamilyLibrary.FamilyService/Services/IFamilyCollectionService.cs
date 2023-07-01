using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services
{
    public interface IFamilyCollectionService : IServiceBase<FamilyCollection>
    {
    }

    public class FamilyCollectionService : ServiceBase<FamilyCollection>, IFamilyCollectionService
    {
        public FamilyCollectionService(IRepositoryBase<FamilyCollection> dbContext) : base(dbContext)
        {
        }
    }
}