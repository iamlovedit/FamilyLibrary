using GalaFamilyLibrary.IdentityService.Models;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public interface IUserService : IServiceBase<LibraryUser>
    {
        Task<List<string>> GetUserRolesByIdAsync(int userId);
    }
}
