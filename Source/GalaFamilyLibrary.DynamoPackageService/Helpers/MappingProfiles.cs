using AutoMapper;
using GalaFamilyLibrary.DynamoPackageService.DataTransferObjects;
using GalaFamilyLibrary.DynamoPackageService.Models;

namespace GalaFamilyLibrary.DynamoPackageService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DynamoPackage, PackageDTO>();
            CreateMap<PackageVersion, PackageVersionDTO>();
        }
    }
}
