using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services
{
    public interface IFamilyParameterService : IServiceBase<FamilyParameter>
    {
        Task<FamilyParameter> GetParameterDetailsAsync(long id);
    }

    public class FamilyParameterService : ServiceBase<FamilyParameter>, IFamilyParameterService
    {
        public FamilyParameterService(IRepositoryBase<FamilyParameter> dbContext) : base(dbContext)
        {

        }

        public async Task<FamilyParameter> GetParameterDetailsAsync(long id)
        {
            return await DAL.DbContext.Queryable<FamilyParameter>().
                   Includes(f => f.Definition, d => d.ParameterGroup).
                   Includes(f => f.Definition, d => d.ParameterType).
                   Includes(f => f.Definition, d => d.UnitType).
                   InSingleAsync(id);
        }
    }
}
