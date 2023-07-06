using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Domain.Models.Identity;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.FamilyLibrary
{
    [SugarTable("families")]
    public class Family : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "family_id")]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "family_name")]
        public string Name { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "family_createTime")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "family_updateTime")]
        public DateTime UpdateTime { get; set; }

        [SugarColumn(ColumnName = "family_stars")]
        public int Stars { get; set; }

        [Navigate(typeof(FamilyCollection), nameof(FamilyCollection.FamilyId), nameof(FamilyCollection.UserId))]
        public List<ApplicationUser> Collectors { get; set; }

        [Navigate(typeof(FamilyStar), nameof(FamilyStar.FamilyId), nameof(FamilyStar.UserId))]
        public List<ApplicationUser> StarredUsers { get; set; }

        [SugarColumn(ColumnName = "family_downloads")]
        public uint Downloads { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(UploaderId))]
        public ApplicationUser Uploader { get; set; }

        [SugarColumn(ColumnName = "family_uploaderId")]
        public long UploaderId { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(FamilySymbol.FamilyId))]
        public List<FamilySymbol> Symbols { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(CategoryId))]
        public FamilyCategory Category { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(ParameterDefinition.FamilyId))]
        public List<ParameterDefinition> ParameterDefinitions { get; set; }

        [SugarColumn(ColumnDataType = "varchar(4000)", IsJson = true, ColumnName = "family_versions")]
        public List<ushort> Versions { get; set; }

        [SugarColumn(ColumnName = "family_categoryId")]
        public long CategoryId { get; set; }

        [SugarColumn(ColumnName = "family_fileId")]
        public string FileId { get; set; }

        [SugarColumn(ColumnName = "family_md5")]
        public string MD5 { get; set; }

        [SugarColumn(ColumnName = "family_isDeleted")]
        public bool IsDeleted { get; set; }

        public string GetFilePath(ushort version)
        {
            return Path.Combine("families", $"{version}", $"{FileId}.rfa");
        }
        public string GetImagePath()
        {
            return Path.Combine("images", $"{FileId}.png");
        }
    }
}
