namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class FamilySymbolDTO
    {
        public string Name { get; set; }

        public List<FamilyParameterDTO> Parameters { get; set; }
    }
}