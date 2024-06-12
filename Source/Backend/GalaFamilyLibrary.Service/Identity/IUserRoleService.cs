using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.Identity
{
    public interface IUserRoleService : IServiceBase<UserRole>
    {

    }

    public class UserRoleService : ServiceBase<UserRole>, IUserRoleService
    {
        public UserRoleService(IRepositoryBase<UserRole> dbContext) : base(dbContext)
        {
        }
    }
}
