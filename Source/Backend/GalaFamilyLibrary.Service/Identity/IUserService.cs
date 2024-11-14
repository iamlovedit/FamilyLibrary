using GalaFamilyLibrary.Model.Identity;

namespace GalaFamilyLibrary.Service.Identity
{
    public interface IUserService : IServiceBase<User, long>
    {
        Task<List<Role>> GetUserRolesAsync(long userId);
    }

    public class UserService(IRepositoryBase<User, long> dbContext) : ServiceBase<User, long>(dbContext), IUserService
    {
        public async Task<List<Role>> GetUserRolesAsync(long userId)
        {
            var user = await DAL.DbContext.Queryable<User>()
                .Includes(u => u.Roles)
                .InSingleAsync(userId);
            return user?.Roles!;
        }
    }
}