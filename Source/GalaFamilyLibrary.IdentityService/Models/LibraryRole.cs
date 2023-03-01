using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.IdentityService.Models
{
    [SugarTable("roles")]
    public class LibraryRole:IDeletable
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Name { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string? Description { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
