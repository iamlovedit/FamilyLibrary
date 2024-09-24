using GalaFamilyLibrary.Model.FamilyLibrary;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyLikeCountService : IServiceBase<FamilyLikeCount>
    {
    }

    public class FamilyLikeCountService(IRepositoryBase<FamilyLikeCount> dbContext)
        : ServiceBase<FamilyLikeCount>(dbContext), IFamilyLikeCountService
    {
        
    }
}