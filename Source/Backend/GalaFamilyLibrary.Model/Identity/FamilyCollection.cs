using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.Identity
{
    [SugarTable(TableName = "library_userfavs")]
    public class FamilyCollections : IDeletable
    {
        [SugarColumn(ColumnName = "favs_familyId", IsPrimaryKey = true)]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "favs_userId", IsPrimaryKey = true)]
        public long UserId { get; set; }

        [SugarColumn(ColumnName = "favs_createDate")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "favs_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
