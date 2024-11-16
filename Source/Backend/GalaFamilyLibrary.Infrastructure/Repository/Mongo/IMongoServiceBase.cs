using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Domains;
using MongoDB.Driver;

namespace GalaFamilyLibrary.Infrastructure.Repository.Mongo;

public interface IMongoServiceBase<TEntity, TKey> where TEntity : class, IIdentifiable<TKey>, new()
    where TKey : IEquatable<TKey>
{
    IMongoRepositoryBase<TEntity, TKey> DAL { get; set; }
    
    Task AddAsync(TEntity entity);

    Task AddManyAsync(IEnumerable<TEntity> entities);
    
    Task<TEntity?> GetAsync(TKey id);

    Task<List<TEntity>?> GetListAsync();

    Task<TEntity?> GetByObjectIdAsync(string id);

    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    public Task<List<TEntity>?> GetListAsync(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>?> GetListFilterAsync(FilterDefinition<TEntity> filter);

    Task<TEntity> UpdateAsync(TKey id, TEntity entity);

    Task<TEntity> DeleteAsync(TKey id);

    Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
    
    Task<PageData<TEntity>?> GetPageDataAsync(int page, int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = false
    );
}