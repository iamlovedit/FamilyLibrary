using GalaFamilyLibrary.Infrastructure.Domains;
using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.Model.Package
{
    [SugarTable("library_package_versions")]
    public class PackageVersion : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, IsNullable = false, ColumnName = "version_version")]
        public string? Version { get; set; }

        [SugarColumn(IsPrimaryKey = true, IsNullable = false, ColumnName = "version_packageId")]
        public string? PackageId { get; set; }

        [SugarColumn(ColumnName = "package_url", IsNullable = true)]
        public string? Url { get; set; }

        [JsonProperty("created")]
        [SugarColumn(ColumnName = "package_createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("scan_status")]
        [SugarColumn(IsNullable = true, ColumnName = "package_scanStatus")]
        public string? ScanStatus { get; set; }

        [SugarColumn(ColumnName = "version_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}