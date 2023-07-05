using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.FamilyParameter
{
    [SugarTable(TableName = "parameter_groups")]
    public class ParameterGroup : IDeletable
    {
        [SugarColumn(ColumnName = "group_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "group_value", IsPrimaryKey = true)]
        public string Value { get; set; }

        [SugarColumn(ColumnName = "group_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "group_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
