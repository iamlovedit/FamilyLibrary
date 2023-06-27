using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("family_parameters")]
    public class FamilyParameter:IDeletable
    {
        [SugarColumn(ColumnName = "parameter_id",IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "parameter_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "parameter_value")]
        public string Value { get; set; }

        [SugarColumn(ColumnName ="parameter_stoargeType")]
        public StorageType StorageType { get; set; }

        [SugarColumn(ColumnName ="parameter_groupId")]
        public int GroupId { get; set; }

        [Navigate(NavigateType.OneToOne,nameof(GroupId))]
        public ParameterGroup Group { get; set; }

        [SugarColumn(ColumnName ="parameter_familyId")]
        public int FamilyId { get; set; }

        [SugarColumn(ColumnName = "parameter_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}