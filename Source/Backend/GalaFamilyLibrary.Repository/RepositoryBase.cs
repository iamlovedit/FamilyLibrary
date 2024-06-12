using System.Linq.Expressions;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        public ISqlSugarClient DbContext => _db;
        private readonly ISqlSugarClient _db;

        public RepositoryBase(ISqlSugarClient context)
        {
            _db = context;
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _db.Queryable<T>().InSingleAsync(id);
        }

        public async Task<long> AddSnowflakeAsync(T entity)
        {
            return await _db.Insertable<T>(entity).ExecuteReturnSnowflakeIdAsync();
        }

        public async Task<IList<long>> AddSnowflakesAsync(IList<T> entities)
        {
            return await _db.Insertable<T>(entities).ExecuteReturnSnowflakeIdListAsync();
        }

        public async Task<PageData<T>> QueryPageAsync(Expression<Func<T, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20,
            Expression<Func<T, object>>? orderExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            RefAsync<int> totalCount = 0;
            var list = await _db.Queryable<T>()
                .OrderByIF(orderExpression != null, orderExpression, orderByType)
                .WhereIF(whereExpression != null, whereExpression)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
            return new PageData<T>(pageIndex, pageCount, totalCount, pageSize, list);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Queryable<T>().ToListAsync();
        }

        public async Task<T> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression)
        {
            return await _db.Queryable<T>().Where(expression).FirstAsync();
        }

        public async Task<bool> UpdateColumnsAsync(T entity, Expression<Func<T, object>> expression)
        {
            return await _db.Updateable<T>(entity).
                UpdateColumns(expression).
                ExecuteCommandHasChangeAsync();
        }
    }
}
