using GalaFamilyLibrary.Repository;
using SqlSugar;

namespace GalaFamilyLibrary.Model.FamilyLibrary
{
    [SugarTable("library_family_versions")]
    public class FamilyVersion : IDeletable
    {
        [SugarColumn(ColumnName = "version_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "version_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "version_familyId")]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "version_createdDate")]
        public DateTime CreatedDate { get; set; }

        [SugarColumn(ColumnName = "version_deleted")]
        public bool IsDeleted { get; set; }
    }
}