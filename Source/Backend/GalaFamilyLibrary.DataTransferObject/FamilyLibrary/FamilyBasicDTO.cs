using GalaFamilyLibrary.DataTransferObject.Identity;
using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.DataTransferObject.FamilyLibrary
{
    public class FamilyDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string? Name { get; set; }

        public FamilyCategoryBasicDTO? Category { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}