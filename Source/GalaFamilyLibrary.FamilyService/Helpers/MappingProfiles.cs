using AutoMapper;
using GalaFamilyLibrary.FamilyService.DataTransferObjects;
using GalaFamilyLibrary.FamilyService.Models;

namespace GalaFamilyLibrary.FamilyService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Family, FamilyDTO>();
            CreateMap<FamilyCategory, FamilyCategoryDTO>();
            CreateMap<FamilyCreationDTO, Family>()
                .ForMember(f => f.CreateTime, options => options.MapFrom(c => DateTime.Now));
        }
    }
}