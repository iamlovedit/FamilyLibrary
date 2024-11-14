using GalaFamilyLibrary.Model.Package;

namespace GalaFamilyLibrary.Service.Package
{
    public interface IVersionService : IServiceBase<PackageVersion, long>
    {
    }

    public class VersionService(IRepositoryBase<PackageVersion, long> dbContext)
        : ServiceBase<PackageVersion, long>(dbContext), IVersionService
    {
    }
}