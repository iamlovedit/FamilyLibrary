using AutoMapper;
using GalaFamilyLibrary.FamilyService.DataTransferObjects;
using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.Infrastructure.FileStorage;

namespace GalaFamilyLibrary.FamilyService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles(FileStorageClient fileStorageClient)
        {
            CreateMap<FamilyCategory, FamilyCategoryDTO>();
            CreateMap<Family, FamilyDTO>().ForMember(fd => fd.ImageUrl, options =>
            {
                options.MapFrom(f => fileStorageClient.GetFileUrl(f.Name, Path.Combine("images", $"{f.FileId}.png")));
            });
            CreateMap<FamilyCreationDTO, Family>()
                .ForMember(f => f.CreateTime, options =>
                {
                    options.MapFrom(c => DateTime.Now);
                })
                .ForMember(f => f.FileId, options =>
                {
                    options.MapFrom(f => Guid.NewGuid().ToString("D"));
                });
            CreateMap<FamilyCallbackCreationDTO, Family>()
                .ForMember(f => f.CreateTime, options => options.MapFrom(fd => DateTime.Now))
                .ForMember(f => f.Versions, options => options.MapFrom(fd => new List<ushort> { fd.Version }));
        }
    }
}