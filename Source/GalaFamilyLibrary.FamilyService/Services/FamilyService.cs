using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services;

public class FamilyService : ServiceBase<Family>, IFamilyService
{
    public FamilyService(IRepositoryBase<Family> dbContext) : base(dbContext)
    {

    }

    public async Task<Family> GetFamilyDetails(int id)
    {
        return await DAL.DbContext.Queryable<Family>().
               Includes(f => f.Category).
               Includes(f => f.Parameters).
               InSingleAsync(id);
    }
}