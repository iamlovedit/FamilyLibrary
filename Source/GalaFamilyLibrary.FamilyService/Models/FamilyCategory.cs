using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("family_categroies")]
    public class FamilyCategory
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Name { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Code { get; set; }

        [SugarColumn(IsNullable = true)]
        public int ParentId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<FamilyCategory> Children { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(ParentId))]
        public FamilyCategory Parent { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(Family.CategoryId))]
        public List<Family> Families { get; set; }
    }
}