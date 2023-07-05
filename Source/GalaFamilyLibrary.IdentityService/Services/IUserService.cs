using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public interface IUserService : IServiceBase<ApplicationUser>
    {
        Task<List<string>> GetUserRolesByIdAsync(long userId);
    }
}
