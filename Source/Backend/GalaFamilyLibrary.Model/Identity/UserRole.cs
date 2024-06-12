using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.Identity
{
    [SugarTable(TableName = "library_user_roles")]
    public class UserRole : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "user_role_id")]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "user_role_userId")]
        public long UserId { get; set; }

        [SugarColumn(ColumnName = "user_role_roleId")]
        public long RoleId { get; set; }

        [SugarColumn(ColumnName = "user_role_isDeleted")]
        public bool IsDeleted { get; set; }

        [SugarColumn(ColumnName = "user_role_createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
