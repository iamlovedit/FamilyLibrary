using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("family_familySymbols")]
    public class FamilySymbol : IDeletable
    {
        [SugarColumn(ColumnName = "symbol_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "symbol_name", IsPrimaryKey = true)]
        public string Name { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(FamilyParameter.FamilySymbolId))]
        public List<FamilyParameter> Parameters { get; set; }

        [SugarColumn(ColumnName = "symbol_familyId")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "symbol_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
