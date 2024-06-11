using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.DynamoPackageService.Services;

public interface IPackageService : IServiceBase<DynamoPackage>
{
    Task<DynamoPackage> GetDynamoPackageByIdAsync(string id);
}