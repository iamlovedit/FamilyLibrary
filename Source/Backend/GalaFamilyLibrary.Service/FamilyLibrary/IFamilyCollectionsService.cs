using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Domains;
using GalaFamilyLibrary.Model.Identity;

namespace GalaFamilyLibrary.Service.FamilyLibrary
{
    public interface IFamilyCollectionsService : IServiceBase<FamilyCollections, long>
    {
        Task<FamilyCollections> GetCollectionByExpressionAsync(Expression<Func<FamilyCollections, bool>> expression);
    }

    public class FamilyCollectionsService(IRepositoryBase<FamilyCollections, long> dbContext)
        : ServiceBase<FamilyCollections, long>(dbContext), IFamilyCollectionsService
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