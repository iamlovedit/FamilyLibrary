using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.DynamoPackageService.Services
{
    public class VersionService(IRepositoryBase<PackageVersion> dbContext)
        : ServiceBase<PackageVersion>(dbContext), IVersionService;
}