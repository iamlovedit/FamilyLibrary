using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable(TableName = "library_family_likes")]
    public class FamilyLikes 
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "family_likes_id")]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "family_likes_userId")]
        public long UserId { get; set; }

        [SugarColumn(ColumnName = "family_likes_familyId")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "family_likes_createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
    }
}