using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.IdentityService.Models
{
    [SugarTable("library_roles")]
    public class LibraryRole : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "role_id")]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "role_name")]
        public string Name { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnName = "role_description")]
        public string? Description { get; set; }

        [SugarColumn(ColumnName = "role_createTime")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "role_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
