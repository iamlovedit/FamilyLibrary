using System.Linq.Expressions;
using SqlSugar;

namespace GalaFamilyLibrary.Infrastructure.Repository;

public interface IRepositoryBase<T> where T : class, new()
{
    ISqlSugarClient DbContext { get; }
   
    Task<T> GetByIdAsync(object id);
   
    Task<T> GetByIdAsync(object id, bool useCache = false);
    
    Task<List<T>> GetByExpressionAsync(Expression<Func<T, bool>> whereExpression);

    Task<IList<T>> GetAllAsync(bool useCache = false);

    Task<List<T>> GetByIdsAsync(object[] idArray);

    Task<int> AddAsync(T entity);

    Task<long> AddSnowflakeAsync(T entity);

    Task<IList<long>> AddSnowflakesAsync(IList<T> entities);

    Task<int> AddAsync(IList<T> entities);

    Task<bool> DeleteByIdAsync(object id);

    Task<bool> DeleteAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> UpdateColumnsAsync(T entity, Expression<Func<T, object>> expression);

    Task<T> GetSingleByIdAsync(object id);

    Task<T> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression);

    Task<Common.PageData<T>> QueryPageAsync(Expression<Func<T, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFields = null);

    Task<IList<T>> QueryByOrderAsync(string field, int count, bool isDesc=false);
}