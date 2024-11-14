using GalaFamilyLibrary.Model.FamilyLibrary;


namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyLikeCountService : IServiceBase<FamilyLikeCount,long>
    {
    }

    public class FamilyLikeCountService(IRepositoryBase<FamilyLikeCount,long> dbContext)
        : ServiceBase<FamilyLikeCount,long>(dbContext), IFamilyLikeCountService
    {
        
    }
}