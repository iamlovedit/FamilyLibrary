using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;
using GalaFamilyLibrary.Domain.Models.Identity;
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

        public ApplicationUserDTO Uploader { get; set; }

        public List<ApplicationUserDTO> Collectors { get; set; }

        public List<ApplicationUserDTO> StarredUsers { get; set; }

        public string ImageUrl { get; set; }

        public int Stars { get; set; }

        public uint Downloads { get; set; }

        public int Favorites { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
