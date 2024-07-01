using GalaFamilyLibrary.Model.Tenant;
using GalaFamilyLibrary.Repository;

namespace GalaFamilyLibrary.Service.Tenant
{
    public interface ITenantService:IServiceBase<SystemTenant>
    {
        public Task SaveTenantAsync(SystemTenant tenant);

        public Task InitTenantDbAsync(SystemTenant tenant);
    }
}