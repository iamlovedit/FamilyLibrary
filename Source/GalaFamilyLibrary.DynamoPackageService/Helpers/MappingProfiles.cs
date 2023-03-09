using AutoMapper;
using GalaFamilyLibrary.DynamoPackageService.DataTransferObjects;
using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;

namespace GalaFamilyLibrary.DynamoPackageService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles(IAESEncryptionService aESEncryptionService)
        {
            CreateMap<DynamoPackage, PackageDTO>();
            CreateMap<PackageVersion, PackageVersionDTO>();
        }
    }
}
