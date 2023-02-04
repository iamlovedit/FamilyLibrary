using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models;

public class Family
{
    [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
    public int Id { get; set; }

    [SugarColumn(IsNullable = false)] public string Name { get; set; }
}