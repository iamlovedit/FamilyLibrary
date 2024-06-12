using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.Identity
{
    [SugarTable(TableName = "library_roles")]
    public class Role : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "role_id")]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "role_name")]
        public string? Name { get; set; }

        [Navigate(typeof(UserRole), nameof(UserRole.RoleId), nameof(UserRole.UserId))]
        public List<User>? Users { get; set; }

        [SugarColumn(Length = 100, IsNullable = true, ColumnName = "role_description")]
        public string? Description { get; set; }

        [SugarColumn(ColumnName = "role_createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "role_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
