namespace GalaFamilyLibrary.FamilyService.Models;

public class FamilyCreationDTO
{
    public string Name { get; set; }
    
    public int CategoryId { get; set; }
    
    public ushort Version { get; set; }
    
    public int UploaderId { get; set; }

    public string MD5 { get; set; }
}