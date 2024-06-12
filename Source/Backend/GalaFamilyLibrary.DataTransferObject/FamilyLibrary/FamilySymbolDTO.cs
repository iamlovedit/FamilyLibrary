using GalaFamilyLibrary.DataTransferObject.FamilyParameter;

namespace GalaFamilyLibrary.DataTransferObject.FamilyLibrary
{
    public class FamilySymbolDTO
    {
        public string? Name { get; set; }

        public List<ParameterDTO>? Parameters { get; set; }
    }
}
