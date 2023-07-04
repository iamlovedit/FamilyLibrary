using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class ParameterDefinitionDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Name { get; set; }

        public long FamilyId { get; set; }

        public ParameterGroupDTO ParameterGroup { get; set; }

        public ParameterTypeDTO ParameterType { get; set; }

        public UnitTypeDTO UnitType { get; set; }
    }
}
