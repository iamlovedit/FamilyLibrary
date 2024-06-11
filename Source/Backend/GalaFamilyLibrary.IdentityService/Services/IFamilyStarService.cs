using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public interface IFamilyStarService : IServiceBase<FamilyStar>
    {
        Task<List<FamilyStar>> GetStaredFamilyAsync(long userId);
    }

    public class FamilyStarService(IRepositoryBase<FamilyStar> dbContext)
        : ServiceBase<FamilyStar>(dbContext), IFamilyStarService
    {
        public async Task<List<FamilyStar>> GetStaredFamilyAsync(long userId)
        {
            return await DAL.DbContext.Queryable<FamilyStar>()
                .Where(fs => fs.UserId == userId)
                .ToListAsync();
        }
    }
}