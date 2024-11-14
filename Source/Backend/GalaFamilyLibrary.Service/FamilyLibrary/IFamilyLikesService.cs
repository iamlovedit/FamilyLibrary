using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Domains;
using GalaFamilyLibrary.Model.FamilyLibrary;

namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyLikesService : IServiceBase<FamilyLikes, long>
    {
        Task<FamilyLikes> GetFamilyLikesByExpressionAsync(Expression<Func<FamilyLikes, bool>> expression);
    }

    public class FamilyLikesService(IRepositoryBase<FamilyLikes, long> dbContext)
        : ServiceBase<FamilyLikes, long>(dbContext), IFamilyLikesService
    {
        public async Task<FamilyLikes> GetFamilyLikesByExpressionAsync(Expression<Func<FamilyLikes, bool>> expression)
        {
            return await DAL.DbContext.Queryable<FamilyLikes>()
                .ClearFilter<IDeletable>()
                .FirstAsync(expression);
        }
    }
}