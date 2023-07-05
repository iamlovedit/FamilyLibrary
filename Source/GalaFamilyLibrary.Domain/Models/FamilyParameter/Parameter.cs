using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;
namespace GalaFamilyLibrary.Domain.Models.FamilyParameter
{
    [SugarTable("family_parameters")]
    public class Parameter : IDeletable
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

        [SugarColumn(ColumnName = "parameter_displayUnitTypeId", IsNullable = true)]
        public long DisplayUnitTypeId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(DisplayUnitTypeId))]
        public DisplayUnitType DisplayUnitType { get; set; }

        [SugarColumn(ColumnName = "parameter_definitionId")]
        public long DefinitionId { get; set; }

        [SugarColumn(ColumnName = "parameter_familySymbolId")]
        public long FamilySymbolId { get; set; }

        [SugarColumn(ColumnName = "parameter_userModifiable")]
        public bool UserModifiable { get; set; }

        [SugarColumn(ColumnName = "parameter_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
