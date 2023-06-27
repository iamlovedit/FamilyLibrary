using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable(TableName = "parameter_groups")]
    public class ParameterGroup : IDeletable
    {
        [SugarColumn(ColumnName = "group_id", IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "group_name")]
        public string Name { get; set; }


        [SugarColumn(ColumnName = "group_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
