using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services;

public class FamilyCategoryService : ServiceBase<FamilyCategory>, IFamilyCategoryService
{
    public FamilyCategoryService(IRepositoryBase<FamilyCategory> dbContext) : base(dbContext)
    {
    }

    public async Task<IList<FamilyCategory>> GetCategoryTreeAsync(int? rootId)
    {
        if (rootId.HasValue)
        {
            return await DAL.DbContext.Queryable<FamilyCategory>()
                .Where(fc => fc.ParentId == rootId)
                .Includes(c => c.Parent)
                .ToTreeAsync(c => c.Children, t => t.ParentId, rootId);
        }
        return await DAL.DbContext.Queryable<FamilyCategory>()
            .Includes(c => c.Parent)
            .ToTreeAsync(c => c.Children, t => t.ParentId, null);
    }
}