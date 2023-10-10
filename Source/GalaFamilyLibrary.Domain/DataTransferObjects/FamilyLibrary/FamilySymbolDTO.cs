using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyParameter;

namespace GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary
{
    public class FamilySymbolDTO
    {

        public string Name { get; set; }

        public List<ParameterDTO> Parameters { get; set; }
    }
}
