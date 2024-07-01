using System.ComponentModel;

namespace GalaFamilyLibrary.Model.Tenant
{
    public enum TenantTypeEnum
    {
        None = 0,
        [Description("Id隔离")]
        Id = 1,
        
        [Description("库隔离")]
        Db = 2,
        
        [Description("表隔离")]
        Tables = 3,
    }
}