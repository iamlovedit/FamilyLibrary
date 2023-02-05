using AutoMapper;
using GalaFamilyLibrary.FamilyService.DataTransferObjetcts;
using GalaFamilyLibrary.FamilyService.Models;

namespace GalaFamilyLibrary.FamilyService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Family, FamilyDto>();
        }
    }
}
