using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;
using SqlSugar.Extensions;
using SqlSugar;
using System.Linq.Expressions;

namespace GalaFamilyLibrary.FamilyService.Services;

public class FamilyService : ServiceBase<Family>, IFamilyService
{
    public FamilyService(IRepositoryBase<Family> dbContext) : base(dbContext)
    {

    }

    public async Task<Family> GetFamilyDetails(long id)
    {
        return await DAL.DbContext.Queryable<Family>().
               Includes(f => f.Category).
               Includes(f => f.Symbols, s => s.Parameters,s=>s.Definition).
               InSingleAsync(id);
    }

    public async Task<PageModel<Family>> GetFamilyPageAsync(Expression<Func<Family, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFields = null)
    {
        RefAsync<int> totalCount = 0;
        var list = await DAL.DbContext.Queryable<Family>()
            .Includes(f => f.Category)
            .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageListAsync(pageIndex, pageSize, totalCount);
        var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
        return new PageModel<Family>(pageIndex, pageCount, totalCount, pageSize, list);
    }
}