using GalaFamilyLibrary.Model.FamilyLibrary;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyLikesService:IServiceBase<FamilyLikes>
    {
        
    }

    public class FamilyLikesService(IRepositoryBase<FamilyLikes> dbContext)
        : ServiceBase<FamilyLikes>(dbContext), IFamilyLikesService
    {
        
    }
    
}