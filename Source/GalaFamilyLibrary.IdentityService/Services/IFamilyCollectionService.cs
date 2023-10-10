using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
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


        Task<PageModel<Family>> GetFamilyPageAsync(long userId, int pageIndex, int pageSize, string orderField);
    }

    public class FamilyCollectionService : ServiceBase<FamilyCollection>, IFamilyCollectionService
    {
        public FamilyCollectionService(IRepositoryBase<FamilyCollection> dbContext) : base(dbContext)
        {
        }
        public async Task<PageModel<Family>> GetFamilyPageAsync(long userId, int pageIndex, int pageSize,
            string orderField)
        {
            RefAsync<int> totalCount = 0;
            var families = await DAL.DbContext.Queryable<Family>()
                  .Includes(f => f.Category)
                  .LeftJoin<FamilyCollection>((f, fc) => f.Id == fc.FamilyId)
                  .Where((f, fc) => fc.UserId == userId && !fc.IsDeleted)
                  .OrderByIF(!string.IsNullOrEmpty(orderField), orderField)
                  .ToPageListAsync(pageIndex, pageSize, totalCount);
            var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
            return new PageModel<Family>(pageIndex, pageCount, totalCount, pageSize, families);
        }
    }
}