using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable(TableName = "library_family_likes")]
    public class FamilyLikeCount
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "likes_count_family_id")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "likes_count_liked")]
        public int Liked { get; set; }

        [SugarColumn(ColumnName = "likes_count_disliked")]
        public int Disliked { get; set; }
    }
}