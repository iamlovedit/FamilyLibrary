using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("family_categories")]
    public class FamilyCategory
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true,ColumnName ="category_id")]
        public int Id { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "category_name")]
        public string Name { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "category_code")]
        public string Code { get; set; }

        [SugarColumn(IsNullable = true, ColumnName = "category_parent")]
        public int ParentId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<FamilyCategory> Children { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(ParentId))]
        public FamilyCategory Parent { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(Family.CategoryId))]
        public List<Family> Families { get; set; }
    }
}