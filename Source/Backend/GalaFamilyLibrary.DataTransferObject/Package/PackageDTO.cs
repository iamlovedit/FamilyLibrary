namespace GalaFamilyLibrary.DataTransferObject.Package
{
    public class PackageDTO
    {
        public string? Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string? Id { get; set; }

        public long Downloads { get; set; }

        public long Votes { get; set; }

        public string? Description { get; set; }

        public List<PackageVersionDTO>? Versions { get; set; }
    }
}
