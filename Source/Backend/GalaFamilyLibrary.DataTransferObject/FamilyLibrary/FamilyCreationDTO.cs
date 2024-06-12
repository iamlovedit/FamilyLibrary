namespace GalaFamilyLibrary.DataTransferObject.FamilyLibrary
{
    public class FamilyCreationDTO
    {
        public string? Name { get; set; }

        public long UploaderId { get; set; }

        public string? Description { get; set; }

        public ushort Version { get; set; }

        public long CategoryId { get; set; }
    }
}
