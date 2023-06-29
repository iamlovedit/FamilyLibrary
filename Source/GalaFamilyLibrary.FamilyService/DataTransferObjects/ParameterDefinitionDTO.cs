namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class ParameterDefinitionDTO
    {
        public string Name { get; set; }

        public ParameterGroupDTO Group { get; set; }

        public ParameterTypeDTO ParameterType { get; set; }

        public UnitTypeDTO UnitType { get; set; }
    }
}
