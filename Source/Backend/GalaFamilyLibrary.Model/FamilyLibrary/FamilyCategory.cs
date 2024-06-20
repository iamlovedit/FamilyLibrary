using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable("library_family_categories")]
    public class FamilyCategory : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "category_id")]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false, IsPrimaryKey = true, ColumnName = "category_code")]
        public string? Code { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "category_name")]
        public string? Name { get; set; }

        [SugarColumn(IsNullable = true, ColumnName = "category_parentId")]
        public long ParentId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<FamilyCategory>? Children { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(ParentId))]
        public FamilyCategory? Parent { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(Family.CategoryId))]
        public List<Family>? Families { get; set; }

        [SugarColumn(ColumnName = "category_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}