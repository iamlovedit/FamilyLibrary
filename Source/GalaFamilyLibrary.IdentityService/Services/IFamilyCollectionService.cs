using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public interface IFamilyCollectionService : IServiceBase<FamilyCollection>
    {
        Task<PageModel<FamilyCollection>> GetFamilyCollectionAsync(long userId, int pageIndex, int pageSize, string orderField);
    }

    public class FamilyCollectionService : ServiceBase<FamilyCollection>, IFamilyCollectionService
    {
        public FamilyCollectionService(IRepositoryBase<FamilyCollection> dbContext) : base(dbContext)
        {
        }

        public async Task<PageModel<FamilyCollection>> GetFamilyCollectionAsync(long userId, int pageIndex, int pageSize,
            string orderField)
        {
            RefAsync<int> totalCount = 0;
            var list = await DAL.DbContext.Queryable<FamilyCollection>()
                .Where(fc => fc.UserId == userId)
                .OrderByIF(!string.IsNullOrEmpty(orderField), orderField)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
            return new PageModel<FamilyCollection>(pageIndex, pageCount, totalCount, pageSize, list);
        }
    }
}