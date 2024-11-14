using GalaFamilyLibrary.Infrastructure.Domains;
using SqlSugar;

namespace GalaFamilyLibrary.Model.Tenant
{
    [SugarTable("library_tenant")]
    public class SystemTenant : IDeletable
    {
        [SugarColumn(ColumnName = "tenant_name")]
        public string Name { get; set; }

        [SugarColumn(IsNullable = false, IsPrimaryKey = true, ColumnName = "tenant_id")]
        public long Id { get; set; }

        [SugarColumn(IsNullable = true, ColumnName = "tenant_connection")]
        public string Connection { get; set; }

        [SugarColumn(IsNullable = true, ColumnName = "tenant_remark")]
        public string Remark { get; set; }

        [SugarColumn(ColumnName = "tenant_deleted")]
        public bool IsDeleted { get; set; }
    }
}