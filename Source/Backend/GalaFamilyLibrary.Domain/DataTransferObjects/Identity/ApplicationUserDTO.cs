using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.DataTransferObjects.Identity
{
    public class ApplicationUserDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
