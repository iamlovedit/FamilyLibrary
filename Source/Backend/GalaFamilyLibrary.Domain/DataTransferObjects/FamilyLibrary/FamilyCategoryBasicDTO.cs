using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary
{
    public class FamilyCategoryBasicDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
