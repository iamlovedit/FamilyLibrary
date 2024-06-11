using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.FamilyService.Services;

public interface IFamilyCategoryService : IServiceBase<FamilyCategory>
{
    Task<IList<FamilyCategory>> GetCategoryTreeAsync(int? rootId);
}