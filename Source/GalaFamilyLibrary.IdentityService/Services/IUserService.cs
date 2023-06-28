using GalaFamilyLibrary.IdentityService.Models;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public interface IUserService : IServiceBase<LibraryUser>
    {
        Task<List<string>> GetUserRolesByIdAsync(long userId);

        Task<bool> UpdateUserLastLoginAsync(LibraryUser user);

        Task<bool> UpdateUserLoginErrorCountAsync(LibraryUser user);
    }
}
