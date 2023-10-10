using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.DynamoPackageService.Services
{
    public class VersionService : ServiceBase<PackageVersion>, IVersionService
    {
        public VersionService(IRepositoryBase<PackageVersion> dbContext) : base(dbContext)
        {
        }
    }
}