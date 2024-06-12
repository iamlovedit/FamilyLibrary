using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.Identity
{
    public interface IRoleService : IServiceBase<Role>
    {

    }

    public class RoleService : ServiceBase<Role>, IRoleService
    {
        public RoleService(IRepositoryBase<Role> dbContext) : base(dbContext)
        {
        }
    }
}
