using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.Identity
{
    [SugarTable(TableName = "family_userStars")]
    public class FamilyStar : IDeletable
    {
        [SugarColumn(ColumnName = "stars_userId", IsPrimaryKey = true)]
        public long UserId { get; set; }

        [SugarColumn(ColumnName = "stars_familyId", IsPrimaryKey = true)]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "stars_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
