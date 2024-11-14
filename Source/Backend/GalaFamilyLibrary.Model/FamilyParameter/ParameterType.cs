using GalaFamilyLibrary.Infrastructure.Domains;
using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyParameter
{
    [SugarTable("library_parameter_types")]
    public class ParameterType : IDeletable
    {
        [SugarColumn(ColumnName = "type_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "type_value", IsPrimaryKey = true)]
        public string? Value { get; set; }

        [SugarColumn(ColumnName = "type_name")]
        public string? Name { get; set; }

        [SugarColumn(ColumnName = "type_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}

