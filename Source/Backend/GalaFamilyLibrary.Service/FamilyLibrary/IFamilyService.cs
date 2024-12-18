using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure;
using GalaFamilyLibrary.Model.FamilyLibrary;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Service.FamilyLibrary;

public interface IFamilyService : IServiceBase<Family, long>
{
    Task<IList<FamilyCategory>?> GetCategoryTreeAsync(int? rootId);

    Task<FamilyDetail?> GetFamilyDetails(long id);

    Task<PageData<Family>> GetFamilyPageAsync(Expression<Func<Family, bool>>? whereExpression, int pageIndex = 1,
        int pageSize = 20, string? orderByFields = null);
}

public class FamilyService(IRepositoryBase<Family, long> dbContext)
    : ServiceBase<Family, long>(dbContext), IFamilyService
{
    public async Task<FamilyDetail?> GetFamilyDetails(long familyId)
    {
        var family = await DAL.DbContext.Queryable<Family>()
            .Includes(f => f.Detail, d => d.Uploader)
            .Includes(f => f.Detail, d => d.Symbols, s => s.Parameters)
            .InSingleAsync(familyId);
        return family.Detail;
    }

    public async Task<PageData<Family>> GetFamilyPageAsync(Expression<Func<Family, bool>>? whereExpression,
        int pageIndex = 1, int pageSize = 20, string? orderByFields = null)
    {
        var orderModels = default(List<OrderByModel>);
        if (!string.IsNullOrEmpty(orderByFields))
        {
            var fieldName = DAL.DbContext.EntityMaintenance.GetDbColumnName<Family>(orderByFields);
            orderModels = OrderByModel.Create(new OrderByModel()
                { FieldName = fieldName, OrderByType = OrderByType.Desc });
        }

        RefAsync<int> totalCount = 0;
        var list = await DAL.DbContext.Queryable<Family>()
            .Includes(f => f.Category)
            .OrderBy(orderModels)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageListAsync(pageIndex, pageSize, totalCount);
        var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
        return new PageData<Family>(pageIndex, pageCount, totalCount, pageSize, list);
    }

    public async Task<IList<FamilyCategory>?> GetCategoryTreeAsync(int? rootId)
    {
        return await DAL.DbContext.Queryable<FamilyCategory>()
            .Includes(c => c.Parent)
            .ToTreeAsync(c => c.Children, t => t.ParentId, rootId);
    }
}