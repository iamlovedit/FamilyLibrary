using System.Linq.Expressions;

namespace GalaFamilyLibrary.Infrastructure.Repository;

/// <summary>
/// <inheritdoc cref="IRepositoryBase{T}"/>
/// </summary>
/// <param name="context"></param>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TId"></typeparam>
public class RepositoryBase<T,TId>(ISqlSugarClient context) : IRepositoryBase<T,TId> where T : class, new() where TId : IEquatable<TId>
{
    public ISqlSugarClient DbContext => context;

    public async Task<T?> GetByIdAsync(TId id)
    {
        return await context.Queryable<T>().InSingleAsync(id);
    }

    public async Task<long> AddSnowflakeAsync(T entity)
    {
        return await context.Insertable<T>(entity).ExecuteReturnSnowflakeIdAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        return await context.Insertable<T>(entity).ExecuteReturnEntityAsync();
    }

    public async Task<List<T>> AddRangeAsync(IList<T> entities)
    {
        return await context.Insertable<T>(entities).ExecuteReturnPkListAsync<T>();
    }

    public async Task<IList<long>> AddSnowflakesAsync(IList<T> entities)
    {
        return await context.Insertable<T>(entities).ExecuteReturnSnowflakeIdListAsync();
    }

    public async Task<PageData<T?>> QueryPageAsync(Expression<Func<T, bool>>? whereExpression, int pageIndex = 1,
        int pageSize = 20,
        Expression<Func<T, object>>? orderExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        RefAsync<int> totalCount = 0;
        var list = await context.Queryable<T>()
            .OrderByIF(orderExpression != null, orderExpression, orderByType)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageListAsync(pageIndex, pageSize, totalCount);
        var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
        return new PageData<T?>(pageIndex, pageCount, totalCount, pageSize, list);
    }

    public async Task<List<T?>> GetAllAsync()
    {
        return await context.Queryable<T?>().ToListAsync();
    }

    public async Task<T?> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression)
    {
        return await context.Queryable<T>().Where(expression).FirstAsync();
    }

    public async Task<bool> UpdateColumnsAsync(T entity, Expression<Func<T, object>> expression)
    {
        return await context.Updateable(entity).UpdateColumns(expression).ExecuteCommandHasChangeAsync();
    }

    public async Task<bool> DeleteByIdAsync(long id)
    {
        return await context.Deleteable<T>().In(id).ExecuteCommandHasChangeAsync();
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        return await context.Deleteable(entity).ExecuteCommandHasChangeAsync();
    }

    public async Task<bool> DeletedByExpressionAsync(Expression<Func<T, bool>> expression)
    {
        return await context.Deleteable<T>().Where(expression).ExecuteCommandHasChangeAsync();
    }
}