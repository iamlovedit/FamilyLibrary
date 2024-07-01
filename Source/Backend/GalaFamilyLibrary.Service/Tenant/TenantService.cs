using GalaFamilyLibrary.Model.Tenant;
using GalaFamilyLibrary.Repository;
using GalaFamilyLibrary.Repository.UnitOfWorks;

namespace GalaFamilyLibrary.Service.Tenant
{
    public class TenantService(IRepositoryBase<SystemTenant> dbContext, IUnitOfWorkManager unitOfWorkManager)
        : ServiceBase<SystemTenant>(dbContext), ITenantService
    {
        public async Task SaveTenantAsync(SystemTenant tenant)
        {
            var initDb = tenant.Id == 0;
            using var unitWork = unitOfWorkManager.CreateUnitWork();
            if (tenant.Id == 0)
            {
                await DAL.DbContext.Insertable(tenant).ExecuteReturnSnowflakeIdAsync();
            }
            else
            {
                var oldTenant = await GetByIdAsync(tenant.Id);
                if (oldTenant.Connection != tenant.Connection)
                {
                    initDb = true;
                }

                await DAL.DbContext.Updateable(tenant).ExecuteCommandAsync();
            }

            unitWork.Commit();
            if (initDb)
            {
                await InitTenantDbAsync(tenant);
            }
        }

        public async Task InitTenantDbAsync(SystemTenant tenant)
        {
        }
    }
}