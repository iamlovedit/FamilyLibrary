using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Infrastructure.Repository;

public class RepositoryBase<T>(ISqlSugarClient context) : IRepositoryBase<T>
    where T : class, new()
{
    public ISqlSugarClient DbContext => context;

    public async Task<T> GetByIdAsync(object id)
    {
        return await context.Queryable<T>().In(id).SingleAsync();
    }

    public async Task<T> GetByIdAsync(object id, bool useCache = false)
    {
        return await context.Queryable<T>().WithCacheIF(useCache).In(id).SingleAsync();
    }

    public async Task<List<T>> GetByExpressionAsync(Expression<Func<T, bool>> whereExpression)
    {
        return await context.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
    }

    public async Task<IList<T>> GetAllAsync(bool useCache = false)
    {
        return await context.Queryable<T>().ToListAsync();
    }

    public async Task<List<T>> GetByIdsAsync(object[] idArray)
    {
        return await context.Queryable<T>().In(idArray).ToListAsync();
    }

    public async Task<long> AddSnowflakeAsync(T entity)
    {
        return await context.Insertable<T>(entity).ExecuteReturnSnowflakeIdAsync();
    }

    public async Task<IList<long>> AddSnowflakesAsync(IList<T> entities)
    {
        return await context.Insertable<T>(entities).ExecuteReturnSnowflakeIdListAsync();
    }

    public async Task<int> AddAsync(T entity)
    {
        var insert = context.Insertable(entity);
        return await insert.ExecuteReturnIdentityAsync();
    }

    public async Task<int> AddAsync(IList<T> entities)
    {
        return await context.Insertable(entities.ToArray()).ExecuteCommandAsync();
    }

    public async Task<bool> DeleteByIdAsync(object id)
    {
        return await context.Deleteable<T>(id).ExecuteCommandHasChangeAsync();
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        return await context.Deleteable<T>(entity).ExecuteCommandHasChangeAsync();
    }

    public async Task<bool> UpdateColumnsAsync(T entity, Expression<Func<T, object>> expression)
    {
        return await context.Updateable<T>(entity).UpdateColumns(expression).ExecuteCommandHasChangeAsync();
    }


    public async Task<bool> UpdateAsync(T entity)
    {
        return await context.Updateable<T>(entity).ExecuteCommandHasChangeAsync();
    }

    public async Task<T> GetSingleByIdAsync(object id)
    {
        return await context.Queryable<T>().InSingleAsync(id);
    }

    public async Task<T> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression)
    {
        return await context.Queryable<T>().FirstAsync(expression);
    }

    public async Task<PageModel<T>> QueryPageAsync(Expression<Func<T, bool>> whereExpression, int pageIndex = 1,
        int pageSize = 20, string? orderByFields = null)
    {
        RefAsync<int> totalCount = 0;
        var list = await context.Queryable<T>()
            .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageListAsync(pageIndex, pageSize, totalCount);
        var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
        return new PageModel<T>(pageIndex, pageCount, totalCount, pageSize, list);
    }

    public async Task<IList<T>> QueryByOrderAsync(string field, int count, bool isDesc = false)
    {
        var method = isDesc ? " desc " : " asc ";
        return await context.Queryable<T>().OrderBy(field + method).Take(count).ToListAsync();
    }
}