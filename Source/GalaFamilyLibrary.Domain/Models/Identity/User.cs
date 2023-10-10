using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.Identity
{
    [SugarTable("application_users")]
    public class User : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "user_id")]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false, Length = 5, ColumnName = "user_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "user_email")]
        public string Email { get; set; }

        [SugarColumn(IsNullable = false, Length = 16, ColumnName = "user_username")]
        public string Username { get; set; }

        [Navigate(typeof(FamilyCollection), nameof(FamilyCollection.UserId), nameof(FamilyCollection.FamilyId))]
        public List<Family> CollectedFamilies { get; set; }

        [Navigate(typeof(FamilyCollection), nameof(FamilyCollection.UserId), nameof(FamilyCollection.FamilyId))]
        public List<Family> StarredFamilies { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "user_password")]
        public string Password { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "user_salt")]
        public string Salt { get; set; }

        [SugarColumn(ColumnName = "user_registerTime")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "user_updateTime")]
        public DateTime UpdateTime { get; set; }

        [SugarColumn(IsNullable = true, ColumnName = "user_lastLogin")]
        public DateTime LastLoginTime { get; set; }

        [SugarColumn(ColumnName = "user_errorCount")]
        public int ErrorCount { get; set; }

        [SugarColumn(ColumnName = "user_isDeleted")]
        public bool IsDeleted { get; set; }

    }
}
