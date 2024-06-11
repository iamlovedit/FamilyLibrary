using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.DynamoPackageService.Services
{
    public interface IVersionService : IServiceBase<PackageVersion>
    {
    }
}