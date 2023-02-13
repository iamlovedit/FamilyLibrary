using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.DynamoPackageService.Services;

public class PackageService : ServiceBase<DynamoPackage>, IPackageService
{
    public PackageService(IRepositoryBase<DynamoPackage> dbContext) : base(dbContext)
    {
    }

    public async Task<DynamoPackage> GetDynamoPackageByIdAsync(string id)
    {
        return await DAL.DbContext.Queryable<DynamoPackage>()
          .Includes(d => d.Versions)
          .FirstAsync(p => p.IsDeleted == false && p.Id == id);
    }
}