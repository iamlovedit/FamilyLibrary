using System.Linq.Expressions;

namespace GalaFamilyLibrary.Infrastructure.Repository;

/// <summary>
/// 数据库ORM仓储
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IRepositoryBase<T, in TId> where T : class, new() where TId : IEquatable<TId>
{
    ISqlSugarClient DbContext { get; }

    Task<T?> GetByIdAsync(TId id);

    Task<List<T?>> GetAllAsync();

    Task<T?> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression);

    Task<long> AddSnowflakeAsync(T entity);

    Task<T> AddAsync(T entity);

    Task<List<T>> AddRangeAsync(IList<T> entities);

    Task<IList<long>> AddSnowflakesAsync(IList<T> entities);

    Task<PageData<T?>> QueryPageAsync(Expression<Func<T, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20,
        Expression<Func<T, object>>? orderExpression = null, OrderByType orderByType = OrderByType.Asc);

    Task<bool> UpdateColumnsAsync(T entity, Expression<Func<T, object>> expression);

    Task<bool> DeleteByIdAsync(long id);

    Task<bool> DeleteAsync(T entity);

    Task<bool> DeletedByExpressionAsync(Expression<Func<T, bool>> expression);
}