using GalaFamilyLibrary.DataTransferObject.Package;
using GalaFamilyLibrary.Infrastructure;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Repository.Mongo;
using GalaFamilyLibrary.Model.Package;
using Mapster;
using SqlSugar;

namespace GalaFamilyLibrary.Service.Package
{
    public interface IPublishedPackageService : IMongoServiceBase<PublishedPackage, string>
    {
        Task<PageData<PublishedPackageDto>> GetPackagePageAsync(string? keyword = null, int pageIndex = 1,
            int pageSize = 30, string? orderBy = null);
    }

    public class PublishedPackageService(
        IMongoRepositoryBase<PublishedPackage, string> repositoryBase,
        IRedisBasketRepository redis)
        : MongoServiceBase<PublishedPackage, string>(repositoryBase), IPublishedPackageService
    {
        public async Task<PageData<PublishedPackageDto>> GetPackagePageAsync(string? keyword = null, int pageIndex = 1,
            int pageSize = 30, string? orderBy = null)
        {
            var expression = Expressionable.Create<PublishedPackage>()
                .AndIF(!string.IsNullOrEmpty(keyword),
                    x => x.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
                .ToExpression();
            var packagePage = await DAL.GetPageDataAsync(pageIndex, pageSize, expression, orderBy);
            return packagePage.ConvertTo<PublishedPackageDto>();
        }
    }
}