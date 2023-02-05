using AutoMapper;
using GalaFamilyLibrary.DynamoPackageService.DataTransferObjetcts;
using GalaFamilyLibrary.DynamoPackageService.Models;

namespace GalaFamilyLibrary.DynamoPackageService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DynamoPackage, PackageDto>();
        }
    }
}
