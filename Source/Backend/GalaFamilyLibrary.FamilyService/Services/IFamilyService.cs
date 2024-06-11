using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Infrastructure.Common;
using System.Linq.Expressions;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.FamilyService.Services;

public interface IFamilyService : IServiceBase<Family>
{
    Task<Family> GetFamilyDetails(long id);

    Task<PageData<Family>> GetFamilyPageAsync(Expression<Func<Family, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFields = null);

}