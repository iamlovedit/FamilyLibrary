using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Domains;
using MongoDB.Driver;

namespace GalaFamilyLibrary.Infrastructure.Repository.Mongo;

public abstract class MongoServiceBase<TEntity, TKey>(IMongoRepositoryBase<TEntity, TKey> repositoryBase)
    : IMongoServiceBase<TEntity, TKey>
    where TEntity : class, IIdentifiable<TKey>, new()
    where TKey : IEquatable<TKey>
{
    public IMongoRepositoryBase<TEntity, TKey> DAL { get; set; } = repositoryBase;

    public async Task AddAsync(TEntity entity)
    {
        await DAL.AddAsync(entity);
    }

    public async Task AddManyAsync(IEnumerable<TEntity> entities)
    {
        await DAL.AddManyAsync(entities);
    }

    public async Task<TEntity?> GetAsync(TKey id)
    {
        return await DAL.GetAsync(id);
    }

    public async Task<List<TEntity>?> GetListAsync()
    {
        return await DAL.GetListAsync();
    }

    public async Task<TEntity?> GetByObjectIdAsync(string id)
    {
        return await DAL.GetByObjectIdAsync(id);
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DAL.GetFirstOrDefaultAsync(predicate);
    }

    public async Task<List<TEntity>?> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DAL.GetListAsync(predicate);
    }

    public async Task<List<TEntity>?> GetListFilterAsync(FilterDefinition<TEntity> filter)
    {
        return await DAL.GetListFilterAsync(filter);
    }

    public async Task<TEntity> UpdateAsync(TKey id, TEntity entity)
    {
        return await DAL.UpdateAsync(id, entity);
    }

    public async Task<TEntity> DeleteAsync(TKey id)
    {
        return await DAL.DeleteAsync(id);
    }

    public async Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DAL.DeleteManyAsync(predicate);
    }

    public async Task<PageData<TEntity>?> GetPageDataAsync(int page, int pageSize,
        Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = false)
    {
        return await DAL.GetPageDataAsync(page, pageSize, filter, orderBy, ascending);
    }

    public async Task<PageData<TEntity>?> GetPageDataAsync(int page, int pageSize,
        Expression<Func<TEntity, bool>>? filter = null, string? orderBy = null,
        bool ascending = false)
    {
        return await DAL.GetPageDataAsync(page, pageSize, filter, orderBy, ascending);
    }
}