namespace GalaFamilyLibrary.Infrastructure.Domains;

/// <summary>
/// 实体类软删除接口，用于filter
/// </summary>
public interface IDeletable
{
    bool IsDeleted { get; set; }
}