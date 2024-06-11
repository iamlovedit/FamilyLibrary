using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Repository;
using SqlSugar;
using SqlSugar.Extensions;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public interface IFamilyCollectionService : IServiceBase<FamilyCollection>
    {


        Task<PageData<Family>> GetFamilyPageAsync(long userId, int pageIndex, int pageSize, string orderField);
    }

    public class FamilyCollectionService(IRepositoryBase<FamilyCollection> dbContext)
        : ServiceBase<FamilyCollection>(dbContext), IFamilyCollectionService
    {
        public async Task<PageData<Family>> GetFamilyPageAsync(long userId, int pageIndex, int pageSize,
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
            return new PageData<Family>(pageIndex, pageCount, totalCount, pageSize, families);
        }
    }
}