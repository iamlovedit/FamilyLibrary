using GalaFamilyLibrary.Infrastructure.Service;
using GalaFamilyLibrary.UserService.Models;

namespace GalaFamilyLibrary.UserService.Services
{
    public interface IUserService : IServiceBase<LibraryUser>
    {
        Task<List<string>> GetUserRolesByIdAsync(int userId);
    }
}
