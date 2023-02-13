using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.Models
{
    [SugarTable("family_parameters")]
    public class FamilyParameter:IDeletable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int DefinitionId { get; set; }

        public int FamilyId { get; set; }
        public bool IsDeleted { get; set; }
    }
}