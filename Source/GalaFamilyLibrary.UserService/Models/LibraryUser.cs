using SqlSugar;

namespace GalaFamilyLibrary.UserService.Models
{
    [SugarTable("users")]
    public class LibraryUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = false, Length = 5)]
        public string Name { get; set; }

        public string Email { get; set; }

        [SugarColumn(IsNullable = false, Length = 16)]
        public string Username { get; set; }

        [SugarColumn(IsNullable = false, Length = 16)]
        public string Password { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public DateTime LastLoginTime { get; set; }

        public bool IsDeleted { get; set; }
            
        public int ErrorCount { get; set; }
    }
}
