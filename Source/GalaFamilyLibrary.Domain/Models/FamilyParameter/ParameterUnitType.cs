using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.FamilyParameter
{
    [SugarTable("parameter_unitType")]
    public class ParameterUnitType : IDeletable
    {
        [SugarColumn(ColumnName = "unitType_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "unitType_value", IsPrimaryKey = true)]
        public string Value { get; set; }

        [SugarColumn(ColumnName = "unitType_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "unitType_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
