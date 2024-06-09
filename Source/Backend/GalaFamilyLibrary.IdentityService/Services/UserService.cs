using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.IdentityService.Services
{
    public class UserService(
        IRepositoryBase<User> dbContext,
        IRepositoryBase<Role> roleRepository,
        IRepositoryBase<UserRole> userRoleRepository)
        : ServiceBase<User>(dbContext), IUserService
    {
        public async Task<User> GetUserCollectionsAsync(long userId)
        {
           return await DAL.DbContext.Queryable<User>()
                .Includes(u => u.CollectedFamilies)
                .InSingleAsync(userId);
        }

        public async Task<List<string>> GetUserRolesByIdAsync(long userId)
        {
            var roleNames = new List<string>();
            var roles = await roleRepository.GetByExpressionAsync(r => !r.IsDeleted);
            var userRoles = await userRoleRepository.GetByExpressionAsync(r => (!r.IsDeleted) && r.UserId == userId);
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
