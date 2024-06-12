using System.Linq.Expressions;
using GalaFamilyLibrary.Repository;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Service.Package
{
    public interface IPackageService : IServiceBase<Model.Package.Package>
    {
        Task<Model.Package.Package> GetPackageDetailByIdAsync(string id);

        Task<PageData<Model.Package.Package>> GetPackagePageAsync(Expression<Func<Model.Package.Package, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderBy = null);
    }

    public class PackageService : ServiceBase<Model.Package.Package>, IPackageService
    {
        public PackageService(IRepositoryBase<Model.Package.Package> dbContext) : base(dbContext)
        {
        }

        public async Task<Model.Package.Package> GetPackageDetailByIdAsync(string id)
        {
            return await DAL.DbContext.Queryable<Model.Package.Package>()
                 .Includes(p => p.Versions)
                 .InSingleAsync(id);
        }

        public async Task<PageData<Model.Package.Package>> GetPackagePageAsync(Expression<Func<Model.Package.Package, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderBy = null)
        {
            var orderModels = default(List<OrderByModel>);
            if (!string.IsNullOrEmpty(orderBy))
            {
                var fieldName = DAL.DbContext.EntityMaintenance.GetDbColumnName<Model.Package.Package>(orderBy);
                orderModels = OrderByModel.Create(new OrderByModel() { FieldName = fieldName, OrderByType = OrderByType.Desc });
            }
            RefAsync<int> totalCount = 0;
            var packages = await DAL.DbContext.Queryable<Model.Package.Package>()
                   .WhereIF(whereExpression != null, whereExpression)
                   .OrderBy(orderModels)
                   .ToPageListAsync(pageIndex, pageSize, totalCount);
            var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
            return new PageData<Model.Package.Package>(pageIndex, pageCount, totalCount, pageSize, packages);
        }
    }
}
