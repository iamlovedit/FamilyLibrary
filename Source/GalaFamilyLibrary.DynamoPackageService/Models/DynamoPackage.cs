using GalaFamilyLibrary.Infrastructure.Common;
using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.DynamoPackageService.Models;

[SugarTable("packages")]
public class DynamoPackage : IDeletable
{
    [SugarColumn(IsPrimaryKey = true)]
    [JsonProperty("_id")]
    public string Id { get; set; }

    [SugarColumn(IsNullable = false)] public string Name { get; set; }

    [JsonProperty("created")] public DateTime CreateTime { get; set; }

    [JsonProperty("latest_version_update")]
    public DateTime UpdateTime { get; set; }

    [SugarColumn(IsNullable = true, Length = 10000)]
    public string? License { get; set; }

    [SugarColumn(IsNullable = true)] public string? Engine { get; set; }

    public long Downloads { get; set; }

    public long Votes { get; set; }
    [SugarColumn(IsNullable = true)] public string? Group { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(PackageVersion.PackageId))]
    public List<PackageVersion> Versions { get; set; }

    public bool IsDeleted { get; set; }
}