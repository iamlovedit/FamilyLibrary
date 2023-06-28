using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.IdentityService.Models
{
    [SugarTable("library_user_roles")]
    public class UserRole : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "user_roles_id")]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "user_roles_userId")]
        public int UserId { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(UserId))]
        public LibraryUser User { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(RoleId))]
        public LibraryRole Role { get; set; }

        [SugarColumn(ColumnName = "user_roles_roleId")]
        public int RoleId { get; set; }

        [SugarColumn(ColumnName = "user_roles_createId")]
        public int CreateId { get; set; }

        [SugarColumn(ColumnName = "user_roles_createTime")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "user_roles_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
