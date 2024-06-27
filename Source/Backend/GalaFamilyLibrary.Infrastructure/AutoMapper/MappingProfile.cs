using AutoMapper;
using GalaFamilyLibrary.DataTransferObject.FamilyLibrary;
using GalaFamilyLibrary.DataTransferObject.FamilyParameter;
using GalaFamilyLibrary.DataTransferObject.Identity;
using GalaFamilyLibrary.DataTransferObject.Package;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Model.FamilyLibrary;
using GalaFamilyLibrary.Model.FamilyParameter;
using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Model.Package;


namespace GalaFamilyLibrary.Infrastructure.AutoMapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FamilyCategory, FamilyCategoryDTO>();
            CreateMap<FamilyCategory, FamilyCategoryBasicDTO>();
            CreateMap<FamilySymbol, FamilySymbolDTO>();
            CreateMap<Family, FamilyDetailDTO>();
            CreateMap<Family, FamilyBasicDTO>();

            CreateMap<DynamoPackage, PackageDTO>();
            CreateMap<PackageVersion, PackageVersionDTO>();

            CreateMap<User, UserDTO>();
            CreateMap<UserCreationDTO, User>().ForMember(u => u.Salt,
                    options => { options.MapFrom((ud, u) => { return u.Salt = Guid.NewGuid().ToString("N"); }); })
                .ForMember(u => u.Password,
                    options => { options.MapFrom((ud, u) => ud.Password!.MD5Encrypt32(u.Salt!)); });

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