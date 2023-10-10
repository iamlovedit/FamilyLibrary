using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.Infrastructure.Service;

public interface IServiceBase<T> where T : class, new()
{
    IRepositoryBase<T> DAL { get; }

    Task<T> GetByIdAsync(object id);

    Task<T> GetByIdAsync(object id, bool useCache = false);

    Task<List<T>> GetByIdsAsync(object[] idArray);

    Task<IList<T>> GetAllAsync();

    Task<IList<T>> GetByExpressionAsync(Expression<Func<T, bool>> expression);

    Task<T> GetSingleByIdAsync(object id);

    Task<T> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression);

    Task<int> AddAsync(T entity);

    Task<int> AddAsync(IList<T> entities);

    Task<long> AddSnowflakeAsync(T entity);

    Task<IList<long>> AddSnowflakesAsync(IList<T> entities);

    Task<bool> DeleteByIdAsync(object id);

    Task<bool> DeleteAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> UpdateColumnsAsync(T entity, Expression<Func<T, object>> expression);

    Task<PageModel<T>> QueryPageAsync(Expression<Func<T, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFields = null);

    Task<IList<T>> QueryByOrderAsync(string field, int count, bool isDesc = false);
}