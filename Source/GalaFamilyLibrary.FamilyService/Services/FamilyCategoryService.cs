using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services;

public class FamilyCategoryService : ServiceBase<FamilyCategory>, IFamilyCategoryService
{
    public FamilyCategoryService(IRepositoryBase<FamilyCategory> dbContext) : base(dbContext)
    {
    }

    public async Task<IList<FamilyCategory>> GetCategoryTreeAsync()
    {
        return await DAL.DbContext.Queryable<FamilyCategory>().
            Includes(c=>c.Parent).
            ToTreeAsync(t => t.Children, t => t.ParentId, null);
    }
}