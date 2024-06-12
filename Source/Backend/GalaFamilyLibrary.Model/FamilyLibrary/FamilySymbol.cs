using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable("library_family_familySymbols")]
    public class FamilySymbol : IDeletable
    {
        [SugarColumn(ColumnName = "symbol_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "symbol_name", IsPrimaryKey = true)]
        public string? Name { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(FamilyParameter.Parameter.FamilySymbolId))]
        public List<FamilyParameter.Parameter>? Parameters { get; set; }

        [SugarColumn(ColumnName = "symbol_familyId")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "symbol_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}