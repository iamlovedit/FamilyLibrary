using GalaFamilyLibrary.Model.Identity;

namespace GalaFamilyLibrary.Service.Identity
{
    public interface IUserRoleService : IServiceBase<UserRole, long>
    {
    }

    public class UserRoleService(IRepositoryBase<UserRole, long> dbContext)
        : ServiceBase<UserRole, long>(dbContext), IUserRoleService
    {
    }
}