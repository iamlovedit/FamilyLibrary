using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("parameter_definitions")]
    public class ParameterDefinition : IDeletable
    {
        [SugarColumn(ColumnName = "definition_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "definition_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "definition_familyId")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "definition_groupId")]
        public long GroupId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(GroupId))]
        public ParameterGroup ParameterGroup { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(ParameterTypeId))]
        public ParameterType ParameterType { get; set; }

        [SugarColumn(ColumnName = "definition_parameterTypeId")]
        public long ParameterTypeId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(UnitTypeId))]
        public ParameterUnitType UnitType { get; set; }

        [SugarColumn(ColumnName = "definition_unitType")]
        public long UnitTypeId { get; set; }

        [SugarColumn(ColumnName = "definition_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
