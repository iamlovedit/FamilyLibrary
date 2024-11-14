using GalaFamilyLibrary.Model.Identity;

namespace GalaFamilyLibrary.Service.Identity
{
    public interface IRoleService : IServiceBase<Role, long>
    {
    }

    public class RoleService : ServiceBase<Role, long>, IRoleService
    {
        public RoleService(IRepositoryBase<Role, long> dbContext) : base(dbContext)
        {
        }
    }
}