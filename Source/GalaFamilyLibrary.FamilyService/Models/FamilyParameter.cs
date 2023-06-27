using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("family_parameters")]
    public class FamilyParameter:IDeletable
    {
        [SugarColumn(ColumnName = "parameter_id")]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "parameter_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "parameter_value")]
        public string Value { get; set; }

        [SugarColumn(ColumnName = "parameter_definitionId")]
        public int DefinitionId { get; set; }

        [SugarColumn(ColumnName = "parameter_familyId")]
        public int FamilyId { get; set; }

        [SugarColumn(ColumnName = "parameter_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}