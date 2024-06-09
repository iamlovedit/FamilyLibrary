using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.DynamoPackageService.Services;

public class PackageService(IRepositoryBase<DynamoPackage> dbContext)
    : ServiceBase<DynamoPackage>(dbContext), IPackageService
{
    public async Task<DynamoPackage> GetDynamoPackageByIdAsync(string id)
    {
        return await DAL.DbContext.Queryable<DynamoPackage>()
          .Includes(d => d.Versions)
          .FirstAsync(p => p.IsDeleted == false && p.Id == id);
    }
}