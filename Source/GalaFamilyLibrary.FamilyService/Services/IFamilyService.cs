using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Service;
using System.Linq.Expressions;

namespace GalaFamilyLibrary.FamilyService.Services;

public interface IFamilyService : IServiceBase<Family>
{
    Task<Family> GetFamilyDetails(long id);

    Task<PageModel<Family>> GetFamilyPageAsync(Expression<Func<Family, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFields = null);

}