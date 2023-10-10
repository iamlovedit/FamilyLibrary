using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.Identity
{
    [SugarTable("library_user_roles")]
    public class UserRole : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "user_roles_id")]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "user_roles_userId")]
        public long UserId { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(UserId))]
        public User User { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(RoleId))]
        public Role Role { get; set; }

        [SugarColumn(ColumnName = "user_roles_roleId")]
        public long RoleId { get; set; }

        [SugarColumn(ColumnName = "user_roles_createId")]
        public long CreateId { get; set; }

        [SugarColumn(ColumnName = "user_roles_createTime")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "user_roles_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
