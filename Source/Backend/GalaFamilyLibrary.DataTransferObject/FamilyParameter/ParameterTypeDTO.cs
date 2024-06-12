using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.DataTransferObject.FamilyParameter
{
    public class ParameterTypeDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }
    }
}

