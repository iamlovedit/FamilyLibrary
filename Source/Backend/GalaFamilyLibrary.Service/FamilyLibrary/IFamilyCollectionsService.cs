using System.Linq.Expressions;
using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyCollectionsService : IServiceBase<FamilyCollections>
    {
        Task<FamilyCollections> GetCollectionByExpressionAsync(Expression<Func<FamilyCollections, bool>> expression);
    }

    public class FamilyCollectionsService(IRepositoryBase<FamilyCollections> dbContext)
        : ServiceBase<FamilyCollections>(dbContext), IFamilyCollectionsService
    {
        public async Task<FamilyCollections> GetCollectionByExpressionAsync(
            Expression<Func<FamilyCollections, bool>> expression)
        {
            return await DAL.DbContext.Queryable<FamilyCollections>()
                .ClearFilter<IDeletable>()
                .FirstAsync(expression);
        }
    }
}