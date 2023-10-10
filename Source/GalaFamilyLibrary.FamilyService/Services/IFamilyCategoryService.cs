using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services;

public interface IFamilyCategoryService : IServiceBase<FamilyCategory>
{
    Task<IList<FamilyCategory>> GetCategoryTreeAsync(int? rootId);
}