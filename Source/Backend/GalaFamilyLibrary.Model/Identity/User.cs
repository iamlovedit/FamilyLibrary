using GalaFamilyLibrary.Infrastructure.Domains;
using GalaFamilyLibrary.Model.FamilyLibrary;
using SqlSugar;

namespace GalaFamilyLibrary.Model.Identity
{
    [SugarTable("library_users")]
    public class User : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "user_id")]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "user_username", Length = 16)]
        public string? Username { get; set; }

        [SugarColumn(ColumnName = "user_nickname", IsNullable = true)]
        public string? Nickname { get; set; }

        [SugarColumn(ColumnName = "user_password", Length = 32)]
        public string? Password { get; set; }

        [SugarColumn(ColumnName = "user_email", Length = 32)]
        public string? Email { get; set; }

        [SugarColumn(ColumnName = "user_salt")]
        public string? Salt { get; set; }

        [Navigate(typeof(UserRole), nameof(UserRole.UserId), nameof(UserRole.RoleId))]
        public List<Role>? Roles { get; set; }

        [Navigate(typeof(FamilyLikes), nameof(FamilyLikes.UserId), nameof(FamilyLikes.FamilyId))]
        public List<FamilyLikes>? Likes { get; set; }

        [Navigate(typeof(FamilyCollections), nameof(FamilyCollections.UserId), nameof(FamilyCollections.FamilyId))]
        public List<FamilyCollections>? Collections { get; set; }

        [SugarColumn(ColumnName = "user_createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "user_lastLoginDate")]
        public DateTime LastLoginDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "user_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}