using AutoMapper;
using GalaFamilyLibrary.Domain.DataTransferObjects.Dynamo;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyParameter;
using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;
using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.FileStorage;

namespace GalaFamilyLibrary.Infrastructure.AutoMapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile(FileStorageClient fileStorageClient)
        {
            CreateMap<FamilyCategory, FamilyCategoryDTO>();
            CreateMap<FamilyCategory, FamilyCategoryBasicDTO>();
            CreateMap<FamilyCreationDTO, Family>()
                .ForMember(f => f.CreateTime, options =>
                {
                    options.MapFrom(c => DateTime.Now);
                })
                .ForMember(f => f.FileId, options =>
                {
                    options.MapFrom(f => Guid.NewGuid().ToString("D"));
                });
            //CreateMap<FamilyCallbackCreationDTO, Family>()
            //    .ForMember(f => f.CreateTime, options => options.MapFrom(fd => DateTime.Now))
            //    .ForMember(f => f.Versions, options => options.MapFrom(fd => new List<ushort> { fd.Version }));

            CreateMap<FamilySymbol, FamilySymbolDTO>();

            CreateMap<Family, FamilyDetailDTO>();
            CreateMap<Family, FamilyBasicDTO>().ForMember(fd => fd.ImageUrl, options =>
            {
                options.MapFrom(f => fileStorageClient.GetFileUrl(f.Name, Path.Combine("images", $"{f.FileId}.png")));
            });


            CreateMap<DynamoPackage, PackageDTO>();
            CreateMap<PackageVersion, PackageVersionDTO>();

            CreateMap<User, ApplicationUserDTO>();
            CreateMap<UserCreationDTO, User>()
                .ForMember(u => u.CreateTime, options => { options.MapFrom((ud, u) => DateTime.Now); })
                .ForMember(u => u.UpdateTime, options => { options.MapFrom((ud, u) => DateTime.Now); })
                .ForMember(u => u.LastLoginTime, options => { options.MapFrom((ud, u) => DateTime.Now); })
                .ForMember(u => u.Salt,
                    options => { options.MapFrom((ud, u) => { return u.Salt = Guid.NewGuid().ToString("N"); }); })
                .ForMember(u => u.Password,
                    options => { options.MapFrom((ud, u) => ud.Password.MD5Encrypt32(u.Salt)); });
            CreateMap<CollectionCreationDTO, FamilyCollection>()
                .ForMember(c => c.CreateTime, options => { options.MapFrom((c, fc) => DateTime.Now); });

            CreateMap<StarCreationDTO, FamilyStar>();

            CreateMap<ParameterGroup, ParameterGroupDTO>();

            CreateMap<Parameter, ParameterDTO>();

            CreateMap<DisplayUnitType, DisplayUnitTypeDTO>();

            CreateMap<ParameterUnitType, UnitTypeDTO>();

            CreateMap<ParameterType, ParameterTypeDTO>();

            CreateMap<ParameterDefinition, ParameterDefinitionDTO>();

            CreateMap<ParameterCreationDTO, Parameter>();

            CreateMap<ParameterDefinitionCreationDTO, ParameterDefinition>();
        }
    }
}
