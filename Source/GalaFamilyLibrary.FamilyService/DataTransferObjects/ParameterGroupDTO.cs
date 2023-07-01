using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class ParameterGroupDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }
        
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
