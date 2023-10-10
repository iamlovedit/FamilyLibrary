namespace GalaFamilyLibrary.Domain.DataTransferObjects.FamilyParameter
{
    public class ParameterDefinitionCreationDTO
    {
        public string Name { get; set; }

        public long FamilyId { get; set; }

        public long GroupId { get; set; }

        public long ParameterTypeId { get; set; }

        public long UnitTypeId { get; set; }
    }
}
