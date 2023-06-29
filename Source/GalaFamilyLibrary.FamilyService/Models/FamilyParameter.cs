using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("family_parameters")]
    public class FamilyParameter : IDeletable
    {
        [SugarColumn(ColumnName = "parameter_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "parameter_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "parameter_value")]
        public string Value { get; set; }

        [SugarColumn(ColumnName = "parameter_stoargeType")]
        public StorageType StorageType { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(DefinitionId))]
        public ParameterDefinition Definition { get; set; }

        [SugarColumn(ColumnName = "parameter_definitionID")]
        public long DefinitionId { get; set; }

        [SugarColumn(ColumnName = "parameter_familySymbolId")]
        public long FamilySymbolId { get; set; }

        [SugarColumn(ColumnName = "parameter_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}