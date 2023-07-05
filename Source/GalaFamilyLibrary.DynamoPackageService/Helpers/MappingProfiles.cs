using AutoMapper;
using GalaFamilyLibrary.Domain.DataTransferObjects.Dynamo;
using GalaFamilyLibrary.Domain.Models.Dynamo;
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
