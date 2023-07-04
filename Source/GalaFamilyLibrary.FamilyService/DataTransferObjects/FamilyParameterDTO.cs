using GalaFamilyLibrary.FamilyService.Models;
using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class FamilyParameterDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public DisplayUnitTypeDTO DisplayUnitType { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public Models.StorageType StorageType { get; set; }

        public bool UserModifiable { get; set; }


        public long DefinitionId { get; set; }

    }
}
