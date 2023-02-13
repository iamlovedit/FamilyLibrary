using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.Infrastructure.Service;

public abstract class ServiceBase<T> : IServiceBase<T> where T : class, new()
{
    public IRepositoryBase<T> DAL { get; }

    public ServiceBase(IRepositoryBase<T> dbContext)
    {
        DAL = dbContext;
    }

    public async Task<T> GetByIdAsync(object id)
    {
        return await DAL.GetByIdAsync(id);
    }

    public async Task<T> GetByIdAsync(object id, bool useCache = false)
    {
        return await DAL.GetByIdAsync(id, useCache);
    }

    public async Task<List<T>> GetByIdsAsync(object[] idArray)
    {
        return await DAL.GetByIdsAsync(idArray);
    }

    public async Task<IList<T>> GetAllAsync()
    {
        return await DAL.GetAllAsync();
    }

    public async Task<IList<T>> GetByExpressionAsync(Expression<Func<T, bool>> expression)
    {
        return await DAL.GetByExpressionAsync(expression);
    }

    public async Task<int> AddAsync(T entity)
    {
        return await DAL.AddAsync(entity);
    }

    public async Task<int> AddAsync(IList<T> entities)
    {
        return await DAL.AddAsync(entities);
    }

    public async Task<bool> DeleteByIdAsync(object id)
    {
        return await DAL.DeleteByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        return await DAL.DeleteAsync(entity);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        return await DAL.UpdateAsync(entity);
    }

    public async Task<PageModel<T>> QueryPageAsync(Expression<Func<T, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string orderByFields = null)
    {
        return await DAL.QueryPageAsync(whereExpression, pageIndex, pageSize, orderByFields);
    }

    public async Task<T> GetSingleByIdAsync(object id)
    {
        return await DAL.GetSingleByIdAsync(id);
    }

    public async Task<T> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression)
    {
        return await DAL.GetFirstByExpressionAsync(expression);
    }

    public async Task<IList<T>> QueryByOrderAsync(string field, int count, bool isDesc = false)
    {
        return await DAL.QueryByOrderAsync(field, count, isDesc);
    }
}