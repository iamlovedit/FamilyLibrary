using System.Text.Json.Serialization;
using GalaFamilyLibrary.DataTransferObject.Identity;
using SqlSugar;

namespace GalaFamilyLibrary.DataTransferObject.FamilyLibrary
{
    public class FamilyDetailDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string? Name { get; set; }

        public UserDTO? Uploader { get; set; }

        public FamilyCategoryBasicDTO? Category { get; set; }

        public List<FamilySymbolDTO>? Symbols { get; set; }

        public List<ushort>? Versions { get; set; }

        public string? FileId { get; set; }

        public int Stars { get; set; }

        public uint Downloads { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Description { get; set; }
    }
}
