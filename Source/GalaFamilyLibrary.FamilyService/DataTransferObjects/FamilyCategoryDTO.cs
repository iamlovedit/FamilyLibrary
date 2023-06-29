namespace GalaFamilyLibrary.FamilyService.DataTransferObjects;

public class FamilyCategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public int ParentId { get; set; }

    public List<FamilyCategoryDTO> Children { get; set; }

    public FamilyCategoryDTO Parent { get; set; }
}