using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services
{
    public interface IFamilyStarService : IServiceBase<FamilyStar>
    {
        Task<List<FamilyStar>> GetStaredFamilyAsync(long userId);
    }

    public class FamilyStarService : ServiceBase<FamilyStar>, IFamilyStarService
    {
        public FamilyStarService(IRepositoryBase<FamilyStar> dbContext) : base(dbContext)
        {
        }

        public async Task<List<FamilyStar>> GetStaredFamilyAsync(long userId)
        {
            return await DAL.DbContext.Queryable<FamilyStar>()
                .Where(fs => fs.UserId == userId)
                .Includes(fs => fs.Family)
                .ToListAsync();
        }
    }
}