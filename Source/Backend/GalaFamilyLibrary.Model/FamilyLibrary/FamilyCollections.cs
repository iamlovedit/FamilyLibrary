using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable(TableName = "library_family_collections")]
    public class FamilyCollections : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "family_collections_id")]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "family_collections_userId")]
        public long UserId { get; set; }

        [SugarColumn(ColumnName = "family_collections_familyId")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "family_collections_createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "family_collections_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}