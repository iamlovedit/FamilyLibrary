using GalaFamilyLibrary.Infrastructure.Domains;
using GalaFamilyLibrary.Model.FamilyParameter;
using GalaFamilyLibrary.Model.Identity;
using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable("library_families")]
    public class Family : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "family_id")]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "family_name")]
        public string? Name { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "family_createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "family_updatedDate")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Navigate(NavigateType.OneToOne, nameof(CategoryId))]
        public FamilyCategory? Category { get; set; }

        [SugarColumn(ColumnName = "family_categoryId")]
        public long CategoryId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(DetailId))]
        public FamilyDetail? Detail { get; set; }

        [SugarColumn(ColumnName = "family_detailId")]
        public long DetailId { get; set; }

        [SugarColumn(ColumnName = "family_uniqueId")]
        public string? UniqueId { get; set; }

        [SugarColumn(ColumnName = "family_isDeleted")]
        public bool IsDeleted { get; set; }

        public string GetFilePath(ushort version)
        {
            return Path.Combine($"{version}", $"{UniqueId}.rfa");
        }

        public string GetImagePath()
        {
            return Path.Combine("images", $"{UniqueId}.png");
        }

        public string GetGltfPath()
        {
            return Path.Combine("gltfs", $"{UniqueId}.gltf");
        }
    }
}