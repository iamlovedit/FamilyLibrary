using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.Service;

namespace GalaFamilyLibrary.FamilyService.Services;

public interface IFamilyService:IServiceBase<Family>
{
    Task<Family> GetFamilyDetails(int id);
}