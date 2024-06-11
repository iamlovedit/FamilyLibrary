using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.Infrastructure.Repository;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public interface IUserService : IServiceBase<User>
    {
        Task<List<string>> GetUserRolesByIdAsync(long userId);

        Task<User> GetUserCollectionsAsync(long userId);
    }
}
