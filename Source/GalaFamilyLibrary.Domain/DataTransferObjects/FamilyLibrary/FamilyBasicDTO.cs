using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary
{
    public class FamilyBasicDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Name { get; set; }

        public FamilyCategoryBasicDTO Category { get; set; }

        public string ImageUrl { get; set; }

        public int Stars { get; set; }

        public uint Downloads { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
