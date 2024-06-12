using System.Text.Json.Serialization;
using GalaFamilyLibrary.DataTransferObject.Identity;
using SqlSugar;

namespace GalaFamilyLibrary.DataTransferObject.FamilyLibrary
{
    public class FamilyBasicDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string? Name { get; set; }

        public FamilyCategoryBasicDTO? Category { get; set; }

        public UserDTO? Uploader { get; set; }

        public List<UserDTO>? Collectors { get; set; }

        public List<UserDTO>? StarredUsers { get; set; }

        public string? ImageUrl { get; set; }

        public int Stars { get; set; }

        public uint Downloads { get; set; }

        public int Favorites { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}