using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services
{
    public interface IFamilyStarService : IServiceBase<FamilyStar>
    {
        Task<List<Family>> GetStaredFamilyAsync(long userId);
    }

    public class FamilyStarService : ServiceBase<FamilyStar>, IFamilyStarService
    {
        public FamilyStarService(IRepositoryBase<FamilyStar> dbContext) : base(dbContext)
        {
        }

        public async Task<List<Family>> GetStaredFamilyAsync(long userId)
        {
            return await DAL.DbContext.Queryable<FamilyStar>()
                .Where(fs => fs.UserId == userId)
                .Includes(fs => fs.Family)
                .Select(fs => fs.Family)
                .ToListAsync();
        }
    }
}