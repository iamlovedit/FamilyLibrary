namespace GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary
{
    public class FamilyCreationDTO
    {
        public string Name { get; set; }

        public long CategoryId { get; set; }
        public ushort Version { get; set; }

        public long UploaderId { get; set; }
    }
}