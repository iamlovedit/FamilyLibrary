using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.Dynamo
{
    [SugarTable("package_versions")]
    public class PackageVersion : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, IsNullable = false)]
        public string Version { get; set; }

        [SugarColumn(IsPrimaryKey = true, IsNullable = false)]
        public string PackageId { get; set; }

        public string Url { get; set; }

        [JsonProperty("created")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("scan_status")]
        [SugarColumn(IsNullable = true)]
        public string ScanStatus { get; set; }

        public bool IsDeleted { get; set; }
    }
}
