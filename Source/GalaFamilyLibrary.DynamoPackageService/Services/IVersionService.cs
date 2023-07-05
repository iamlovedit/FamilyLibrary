using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.DynamoPackageService.Services
{
    public interface IVersionService : IServiceBase<PackageVersion>
    {
    }
}