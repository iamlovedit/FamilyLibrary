using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.DynamoPackageService.Services;

public interface IPackageService:IServiceBase<DynamoPackage>
{
    Task<DynamoPackage> GetDynamoPackageByIdAsync(string id);
}