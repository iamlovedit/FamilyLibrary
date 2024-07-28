using System.Linq.Expressions;
using GalaFamilyLibrary.Model.FamilyLibrary;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyLikesService : IServiceBase<FamilyLikes>
    {
        Task<FamilyLikes> GetFamilyLikesByExpressionAsync(Expression<Func<FamilyLikes, bool>> expression);
    }

    public class FamilyLikesService(IRepositoryBase<FamilyLikes> dbContext)
        : ServiceBase<FamilyLikes>(dbContext), IFamilyLikesService
    {
        public async Task<FamilyLikes> GetFamilyLikesByExpressionAsync(Expression<Func<FamilyLikes, bool>> expression)
        {
            return await DAL.DbContext.Queryable<FamilyLikes>()
                .ClearFilter<IDeletable>()
                .FirstAsync(expression);
        }
    }
}