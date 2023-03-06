using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models;

[SugarTable("families")]
public class Family : IDeletable
{
    [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
    public int Id { get; set; }

    [SugarColumn(IsNullable = false)] public string Name { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }

    //[Navigate(NavigateType.OneToOne, nameof(UploaderId))]
    //public LibraryUser User { get; set; }

    public int UploaderId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(CategoryId))]
    public FamilyCategory Category { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(FamilyParameter.FamilyId))]
    public List<FamilyParameter> Parameters { get; set; }

    [SugarColumn(ColumnDataType = "varchar(4000)", IsJson = true)]
    public List<ushort> Versions { get; set; }

    public int CategoryId { get; set; }

    public string FileId { get; set; }

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