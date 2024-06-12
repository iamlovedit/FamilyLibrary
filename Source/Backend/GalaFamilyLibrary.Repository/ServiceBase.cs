using System.Linq.Expressions;
using SqlSugar;

namespace GalaFamilyLibrary.Repository
{
    public abstract class ServiceBase<T> : IServiceBase<T> where T : class, new()
    {
        public IRepositoryBase<T> DAL { get; }

        public ServiceBase(IRepositoryBase<T> dbContext)
        {
            DAL = dbContext;
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await DAL.GetByIdAsync(id);
        }

        public async Task<long> AddSnowflakeAsync(T entity)
        {
            return await DAL.AddSnowflakeAsync(entity);
        }

        public async Task<IList<long>> AddSnowflakesAsync(IList<T> entities)
        {
            return await DAL.AddSnowflakesAsync(entities);
        }

        public async Task<PageData<T>> QueryPageAsync(Expression<Func<T, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20,
            Expression<Func<T, object>>? orderExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            return await DAL.QueryPageAsync(whereExpression, pageIndex, pageSize, orderExpression, orderByType);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await DAL.GetAllAsync();
        }

        public async Task<T> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression)
        {
            return await DAL.GetFirstByExpressionAsync(expression);
        }

        public async Task<bool> UpdateColumnsAsync(T entity, Expression<Func<T, object>> expression)
        {
            return await DAL.UpdateColumnsAsync(entity, expression);
        }
    }
}
