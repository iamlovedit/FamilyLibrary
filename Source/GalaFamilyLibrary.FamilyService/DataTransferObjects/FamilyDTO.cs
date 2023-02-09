namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class FamilyDTO
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public ushort Version { get; set; }

        public int UploaderId { get; set; }

        public FamilyCategoryDTO Category { get; set; }

        public DateTime CreateTime { get; set; }

        public string? Description { get; set; }
    }
}