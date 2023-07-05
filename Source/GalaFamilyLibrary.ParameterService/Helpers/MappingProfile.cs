using AutoMapper;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyParameter;
using GalaFamilyLibrary.Domain.Models.FamilyParameter;

namespace GalaFamilyLibrary.ParameterService.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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
