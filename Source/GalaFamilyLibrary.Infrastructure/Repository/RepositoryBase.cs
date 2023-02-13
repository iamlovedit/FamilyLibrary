using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Infrastructure.Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        public ISqlSugarClient DbContext => _db;
        private readonly ISqlSugarClient _db;

        public RepositoryBase(ISqlSugarClient context)
        {
            _db = context;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _db.Queryable<T>().In(id).SingleAsync();
        }

        public async Task<T> GetByIdAsync(object id, bool useCache = false)
        {
            return await _db.Queryable<T>().WithCacheIF(useCache).In(id).SingleAsync();
        }

        public async Task<List<T>> GetByExpressionAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await _db.Queryable<T>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        public async Task<IList<T>> GetAllAsync(bool useCache = false)
        {
            return await _db.Queryable<T>().ToListAsync();
        }

        public async Task<List<T>> GetByIdsAsync(object[] idArray)
        {
            return await _db.Queryable<T>().In(idArray).ToListAsync();
        }

        public async Task<int> AddAsync(T entity)
        {
            var insert = _db.Insertable(entity);
            return await insert.ExecuteReturnIdentityAsync();
        }

        public async Task<int> AddAsync(IList<T> entities)
        {
            return await _db.Insertable(entities.ToArray()).ExecuteCommandAsync();
        }

        public async Task<bool> DeleteByIdAsync(object id)
        {
            return await _db.Deleteable<T>(id).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            return await _db.Deleteable<T>(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            return await _db.Updateable<T>(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<T> GetSingleByIdAsync(object id)
        {
            return await _db.Queryable<T>().InSingleAsync(id);
        }

        public async Task<T> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression)
        {
            return await _db.Queryable<T>().FirstAsync(expression);
        }

        public async Task<PageModel<T>> QueryPageAsync(Expression<Func<T, bool>> whereExpression, int pageIndex = 1,
            int pageSize = 20, string orderByFields = null)
        {
            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<T>()
                .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
                .WhereIF(whereExpression != null, whereExpression)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
            return new PageModel<T>(pageIndex, pageCount, totalCount, pageSize, list);
        }

        public async Task<IList<T>> QueryByOrderAsync(string field, int count, bool isDesc=false)
        {
            var method = isDesc ? " desc ":" asc ";
            return await _db.Queryable<T>().OrderBy(field + method).Take(count).ToListAsync();
        }
    }