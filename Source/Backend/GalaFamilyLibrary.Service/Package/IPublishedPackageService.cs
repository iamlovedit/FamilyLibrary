using GalaFamilyLibrary.Infrastructure.Repository.Mongo;
using GalaFamilyLibrary.Model.Package;

namespace GalaFamilyLibrary.Service.Package
{
    public interface IPublishedPackageService : IMongoServiceBase<PublishedPackage, string>
    {
    }

    public class PublishedPackageService(IMongoRepositoryBase<PublishedPackage, string> repositoryBase)
        : MongoServiceBase<PublishedPackage, string>(repositoryBase), IPublishedPackageService
    {
        
    }
}