using GalaFamilyLibrary.Infrastructure.Common;
using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.DynamoPackageService.Models
{
    [SugarTable("package_versions")]
    public class PackageVersion : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true)] public string Id { get; set; }

        public string Version { get; set; }

        [SugarColumn(IsNullable = false)] public string PackageId { get; set; }

        public string Url { get; set; }

        [JsonProperty("created")] public DateTime CreateTime { get; set; }

        [JsonProperty("scan_status")]
        [SugarColumn(IsNullable = true)]
        public string ScanStatus { get; set; }

        public bool IsDeleted { get; set; }
    }
}