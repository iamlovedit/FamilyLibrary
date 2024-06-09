using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.DynamoPackageService.Services
{
    public class VersionService(IRepositoryBase<PackageVersion> dbContext)
        : ServiceBase<PackageVersion>(dbContext), IVersionService;
}