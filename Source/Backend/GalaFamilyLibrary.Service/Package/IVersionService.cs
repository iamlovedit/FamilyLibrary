using GalaFamilyLibrary.Model.Package;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.Package
{
    public interface IVersionService : IServiceBase<PackageVersion>
    {
    }
    public class VersionService : ServiceBase<PackageVersion>, IVersionService
    {
        public VersionService(IRepositoryBase<PackageVersion> dbContext) : base(dbContext)
        {
        }
    }
}
