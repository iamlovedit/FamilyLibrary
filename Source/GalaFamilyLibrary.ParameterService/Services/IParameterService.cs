using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.ParameterService.Services
{
    public interface IParameterService : IServiceBase<Parameter>
    {
        Task<Parameter> GetParameterDetailsAsync(long id);
    }

    class ParameterService : ServiceBase<Parameter>, IParameterService
    {
        public ParameterService(IRepositoryBase<Parameter> dbContext) : base(dbContext)
        {
        }

        public async Task<Parameter> GetParameterDetailsAsync(long id)
        {
            return await DAL.DbContext.Queryable<Parameter>().
                   Includes(f => f.Definition, d => d.ParameterGroup).
                   Includes(f => f.Definition, d => d.ParameterType).
                   Includes(f => f.Definition, d => d.UnitType).
                   Includes(f => f.DisplayUnitType).
                   InSingleAsync(id);
        }
    }
}
