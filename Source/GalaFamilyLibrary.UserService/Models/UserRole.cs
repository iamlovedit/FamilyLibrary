using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.UserService.Models
{
    [SugarTable("user_roles")]
    public class UserRole:IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public int UserId { get; set; }
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(UserId))]
        public LibraryUser User { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(RoleId))]
        public LibraryRole Role { get; set; }

        public int RoleId { get; set; }

        public int CreateId { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
