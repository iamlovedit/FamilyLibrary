using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.DynamoPackageService.Services;

public class PackageService:ServiceBase<DynamoPackage>,IPackageService
{
    public PackageService(IRepositoryBase<DynamoPackage> dbContext) : base(dbContext)
    {
    }
}