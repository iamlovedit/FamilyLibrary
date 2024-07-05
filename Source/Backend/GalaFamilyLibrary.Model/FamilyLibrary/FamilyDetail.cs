using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable("library_family_details")]
    public class FamilyDetail : IDeletable
    {
        [SugarColumn(ColumnName = "detail_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "detail_familyId")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "detail_stars")]
        public long Stars { get; set; }

        [SugarColumn(ColumnName = "detail_views")]
        public long Views { get; set; }

        [SugarColumn(ColumnName = "detail_downloads")]
        public long Downloads { get; set; }

        [SugarColumn(ColumnName = "detail_favorites")]
        public long Favorites { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(FamilySymbol.FamilyId))]
        public List<FamilySymbol> Symbols { get; set; }

        [SugarColumn(ColumnName = "detail_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}