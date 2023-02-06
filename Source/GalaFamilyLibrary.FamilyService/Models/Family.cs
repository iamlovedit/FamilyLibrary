using GalaFamilyLibrary.UserService.Models;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models;

[SugarTable("families")]
public class Family
{
    [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
    public int Id { get; set; }

    [SugarColumn(IsNullable = false)]
    public string Name { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(UploaderId))]
    public LibraryUser User { get; set; }

    public int UploaderId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(CategoryId))]
    public FamilyCategory Category { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(FamilyParameter.FamilyId))]
    public List<FamilyParameter> Parameters { get; set; }

    [SugarColumn(ColumnDataType = "varchar(4000)", IsJson = true)]
    public List<ushort> Versions { get; set; }

    public int CategoryId { get; set; }
}