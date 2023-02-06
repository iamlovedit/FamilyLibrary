using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("family_parameters")]
    public class FamilyParameter
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int DefinitionId { get; set; }

        public int FamilyId { get; set; }
    }
}