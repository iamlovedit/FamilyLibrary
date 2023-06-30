using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models;

[SugarTable("families")]
public class Family : IDeletable
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "family_id")]
    public long Id { get; set; }

    [SugarColumn(IsNullable = false, ColumnName = "family_name")]
    public string Name { get; set; }

    [SugarColumn(IsNullable = false, ColumnName = "family_createTime")]
    public DateTime CreateTime { get; set; }

    [SugarColumn(ColumnName = "family_updateTime")]
    public DateTime UpdateTime { get; set; }

    [SugarColumn(ColumnName = "family_stars")]
    public int Stars { get; set; }

    [SugarColumn(ColumnName = "family_downloads")]
    public uint Downloads { get; set; }

    //[Navigate(NavigateType.OneToOne, nameof(UploaderId))]
    //public LibraryUser User { get; set; }

    [SugarColumn(ColumnName = "family_uploaderId")]
    public long UploaderId { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(FamilySymbol.FamilyId))]
    public List<FamilySymbol> Symbols { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(CategoryId))]
    public FamilyCategory Category { get; set; }


    [SugarColumn(ColumnDataType = "varchar(4000)", IsJson = true, ColumnName = "family_versions")]
    public List<ushort> Versions { get; set; }

    [SugarColumn(ColumnName = "family_categoryId")]
    public long CategoryId { get; set; }

    [SugarColumn(ColumnName = "family_fileId")]
    public string FileId { get; set; }

    [SugarColumn(ColumnName = "family_isDeleted")]
    public bool IsDeleted { get; set; }

    internal string GetFilePath(IWebHostEnvironment environment, ushort version)
    {
        return Path.Combine(environment.WebRootPath, "families", $"{version}", $"{FileId}.rfa");
    }

    internal string GetFilePath(ushort version)
    {
        return Path.Combine("families", $"{version}", $"{FileId}.rfa");
    }
    internal string GetImagePath()
    {
        return Path.Combine("images", $"{FileId}.png");
    }

    internal string GetImagePath(IWebHostEnvironment environment)
    {
        return Path.Combine(environment.WebRootPath, "images", $"{FileId}.png");
    }
}