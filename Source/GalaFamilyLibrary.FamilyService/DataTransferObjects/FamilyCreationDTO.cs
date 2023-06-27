namespace GalaFamilyLibrary.FamilyService.DataTransferObjects;

public class FamilyCreationDTO
{
    public string Name { get; set; }

    public int CategoryId { get; set; }

    public ushort Version { get; set; }

    public int UploaderId { get; set; }
}
public class FamilyCallbackCreationDTO : FamilyCreationDTO
{
    public string FileId { get; set; }
}