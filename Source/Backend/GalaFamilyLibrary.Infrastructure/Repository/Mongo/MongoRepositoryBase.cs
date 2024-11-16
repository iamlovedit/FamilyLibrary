using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Domains;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GalaFamilyLibrary.Infrastructure.Repository.Mongo;

public class MongoRepositoryBase<TEntity, TKey>(IMongoDatabase database) :
    IMongoRepositoryBase<TEntity, TKey>
    where TEntity : class, IIdentifiable<TKey>, new()
    where TKey : IEquatable<TKey>
{
    private readonly IMongoCollection<TEntity> _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);

    public IMongoCollection<TEntity> Collection => _collection;

    private const string _idField = "_id";

    public async Task AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task AddManyAsync(IEnumerable<TEntity> entities)
    {
        await _collection.InsertManyAsync(entities);
    }

    public async Task<TEntity?> GetAsync(TKey id)
    {
        var filter = Builders<TEntity>.Filter.Eq(_idField, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>?> GetListAsync()
    {
        return await _collection.AsQueryable().ToListAsync();
    }

    public async Task<TEntity?> GetByObjectIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(_idField, ObjectId.Parse(id));

        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.Find(predicate).FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>?> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.Find(predicate).ToListAsync();
    }


    public async Task<List<TEntity>?> GetListFilterAsync(FilterDefinition<TEntity> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<TEntity> UpdateAsync(TKey id, TEntity entity)
    {
        var filter = Builders<TEntity>.Filter.Eq(_idField, id);
        return await _collection.FindOneAndReplaceAsync(filter, entity);
    }

    public async Task<TEntity> DeleteAsync(TKey id)
    {
        var filter = Builders<TEntity>.Filter.Eq(_idField, id);
        return await _collection.FindOneAndDeleteAsync(filter);
    }

    public async Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var filter = Builders<TEntity>.Filter.Where(predicate);
        var result = await _collection.DeleteManyAsync(filter);
        return result.IsAcknowledged;
    }

    public async Task<PageData<TEntity>?> GetPageDataAsync(int page, int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null, bool ascending = false
    )
    {
        if (pageSize < 1)
        {
            throw new ArgumentException("Page size must be greater than 0.");
        }

        if (page < 1)
        {
            throw new ArgumentException("Page index must be greater than 0.");
        }

        var query = _collection.Find(filter is null
            ? Builders<TEntity>.Filter.Empty
            : Builders<TEntity>.Filter.Where(filter));
        var totalTask = query.CountDocumentsAsync();
        var itemTask = query.Skip((page - 1) * pageSize).Limit(pageSize);
        if (orderBy != null)
        {
            if (ascending)
            {
                itemTask.SortByDescending(orderBy);
            }
            else
            {
                itemTask.SortBy(orderBy);
            }
        }

        var items = await itemTask.ToListAsync();
        var total = await totalTask;
        var pageCount = Math.Ceiling(total.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
        return new PageData<TEntity>(page, pageCount, total, pageSize, items);
    }
}