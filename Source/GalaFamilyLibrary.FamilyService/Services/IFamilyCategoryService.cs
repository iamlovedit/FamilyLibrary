using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services;

public interface IFamilyCategoryService:IServiceBase<FamilyCategory>
{
    Task<IList<FamilyCategory>> GetCategoryTreeAsync(int? rootId);
}