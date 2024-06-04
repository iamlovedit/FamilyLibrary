using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.Identity
{
    [SugarTable("library_role_menus")]
    public class RoleMenu : IDeletable
    {
        [SugarColumn(ColumnName = "role_menu_id")]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "role_menu_roleId")]
        public long RoleId { get; set; }

        [SugarColumn(ColumnName = "role_menu_menuId")]
        public long MenuId { get; set; }

        public bool IsDeleted { get; set; }
    }
}