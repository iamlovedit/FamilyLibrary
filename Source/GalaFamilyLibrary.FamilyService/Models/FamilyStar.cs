using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable(TableName = "family_userStars")]
    public class FamilyStar : IDeletable
    {
        [SugarColumn(ColumnName = "stars_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "stars_userId")]
        public long UserId { get; set; }

        [SugarColumn(ColumnName = "stars_familyId")]
        public long FamilyId { get; set; }

        [Navigate(NavigateType.OneToOne,nameof(FamilyId))]
        public Family Family { get; set; }

        [SugarColumn(ColumnName = "stars_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}