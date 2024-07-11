using GalaFamilyLibrary.Model.Identity;
using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable("library_family_details")]
    public class FamilyDetail : IDeletable
    {
        [SugarColumn(ColumnName = "detail_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(UploaderId))]
        public User? Uploader { get; set; }

        [SugarColumn(ColumnName = "detail_uploaderId")]
        public long UploaderId { get; set; }

        [SugarColumn(ColumnName = "detail_stars")]
        public long Stars { get; set; }

        [SugarColumn(ColumnName = "detail_views")]
        public long Views { get; set; }

        [SugarColumn(ColumnName = "detail_downloads")]
        public long Downloads { get; set; }

        [SugarColumn(ColumnName = "detail_favorites")]
        public long Favorites { get; set; }
    
        [SugarColumn(ColumnDataType = "varchar(4000)", IsJson = true, ColumnName = "family_versions")]
        public List<ushort>? Versions { get; set; }
        
        [Navigate(NavigateType.OneToMany, nameof(FamilySymbol.FamilyId))]
        public List<FamilySymbol> Symbols { get; set; }

        [SugarColumn(ColumnName = "detail_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}