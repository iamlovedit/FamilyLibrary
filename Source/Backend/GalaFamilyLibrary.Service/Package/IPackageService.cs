﻿using System.Linq.Expressions;
using GalaFamilyLibrary.Model.Package;
using GalaFamilyLibrary.Repository;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Service.Package
{
    public interface IPackageService : IServiceBase<DynamoPackage>
    {
        Task<DynamoPackage> GetPackageDetailByIdAsync(string id);

        Task<PageData<DynamoPackage>> GetPackagePageAsync(Expression<Func<DynamoPackage, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderBy = null);
    }

    public class PackageService : ServiceBase<DynamoPackage>, IPackageService
    {
        public PackageService(IRepositoryBase<DynamoPackage> dbContext) : base(dbContext)
        {
        }

        public async Task<DynamoPackage> GetPackageDetailByIdAsync(string id)
        {
            return await DAL.DbContext.Queryable<DynamoPackage>()
                 .Includes(p => p.Versions)
                 .InSingleAsync(id);
        }

        public async Task<PageData<DynamoPackage>> GetPackagePageAsync(Expression<Func<DynamoPackage, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderBy = null)
        {
            var orderModels = default(List<OrderByModel>);
            if (!string.IsNullOrEmpty(orderBy))
            {
                var fieldName = DAL.DbContext.EntityMaintenance.GetDbColumnName<DynamoPackage>(orderBy);
                orderModels = OrderByModel.Create(new OrderByModel() { FieldName = fieldName, OrderByType = OrderByType.Desc });
            }
            RefAsync<int> totalCount = 0;
            var packages = await DAL.DbContext.Queryable<DynamoPackage>()
                   .WhereIF(whereExpression != null, whereExpression)
                   .OrderBy(orderModels)
                   .ToPageListAsync(pageIndex, pageSize, totalCount);
            var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
            return new PageData<DynamoPackage>(pageIndex, pageCount, totalCount, pageSize, packages);
        }
    }
}
