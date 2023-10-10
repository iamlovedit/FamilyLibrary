using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.Identity
{
    [SugarTable("library_menus")]
    public class Menu : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "menu_id")]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "menu_name")]
        public string? Name { get; set; }

        [SugarColumn(ColumnName = "menu_code")]
        public string? Code { get; set; }

        [SugarColumn(ColumnName = "menu_parentId")]
        public long ParentId { get; set; }

        [Navigate(NavigateType.OneToOne, nameof(ParentId))]
        public Menu? Parent { get; set; }

        public List<Menu>? Children { get; set; }

        [SugarColumn(ColumnName = "menu_route")]
        public string? Route { get; set; }

        [SugarColumn(ColumnName = "menu_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}