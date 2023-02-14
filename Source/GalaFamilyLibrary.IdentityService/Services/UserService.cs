using GalaFamilyLibrary.IdentityService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public class UserService : ServiceBase<LibraryUser>, IUserService
    {
        private readonly IRepositoryBase<LibraryRole> _roleRepository;
        private readonly IRepositoryBase<UserRole> _userRoleRepository;

        public UserService(IRepositoryBase<LibraryUser> dbContext, IRepositoryBase<LibraryRole> roleRepository, IRepositoryBase<UserRole> userRoleRepository) : base(dbContext)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<string>> GetUserRolesByIdAsync(int userId)
        {
            var roleNames = new List<string>();
            var roles = await _roleRepository.GetByExpressionAsync(r => !r.IsDeleted);
            var userRoles = await _userRoleRepository.GetByExpressionAsync(r => (!r.IsDeleted) && r.UserId == userId);
            if (userRoles?.Any() ?? false)
            {
                foreach (var role in roles)
                {
                    if (userRoles.Any(u => u.RoleId == role.Id))
                    {
                        roleNames.Add(role.Name);
                    }
                }
            }
            return roleNames;
        }
    }
}
