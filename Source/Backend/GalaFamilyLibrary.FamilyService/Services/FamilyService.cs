using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Repository;
using SqlSugar;
using SqlSugar.Extensions;
using System.Linq.Expressions;

namespace GalaFamilyLibrary.FamilyService.Services;

public class FamilyService(IRepositoryBase<Family> dbContext) : ServiceBase<Family>(dbContext), IFamilyService
{
    public async Task<Family> GetFamilyDetails(long id)
    {
        return await DAL.DbContext.Queryable<Family>().
               Includes(f => f.Category).
               Includes(f => f.Symbols, s => s.Parameters, s => s.DisplayUnitType).
               InSingleAsync(id);
    }

    public async Task<PageData<Family>> GetFamilyPageAsync(Expression<Func<Family, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFields = null)
    {
        RefAsync<int> totalCount = 0;
        var list = await DAL.DbContext.Queryable<Family>()
            .Includes(f => f.Uploader)
            .Includes(f => f.Category)
            .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageListAsync(pageIndex, pageSize, totalCount);
        var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
        return new PageData<Family>(pageIndex, pageCount, totalCount, pageSize, list);
    }
}