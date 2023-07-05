using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.FamilyLibrary
{
    [SugarTable(TableName = "family_userCollections")]
    public class FamilyCollection : IDeletable
    {
        [SugarColumn(ColumnName = "collection_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "collection_familyId")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "collection_userId")]
        public long UserId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(FamilyId))]
        public Family Family { get; set; }

        [SugarColumn(ColumnName = "collection_createTime")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "collection_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
