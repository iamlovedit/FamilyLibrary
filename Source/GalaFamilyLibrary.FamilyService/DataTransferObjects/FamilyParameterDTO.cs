using GalaFamilyLibrary.FamilyService.Models;

namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class FamilyParameterDTO
    {
        public string Name { get; set; }

        public StorageType StorageType { get; set; }

        public string Value { get; set; }

        public ParameterDefinitionDTO Definition { get; set; }
    }
}
