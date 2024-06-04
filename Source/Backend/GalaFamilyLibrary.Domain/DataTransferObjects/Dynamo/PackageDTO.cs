namespace GalaFamilyLibrary.Domain.DataTransferObjects.Dynamo
{
    public class PackageDTO
    {
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Id { get; set; }

        public long Downloads { get; set; }

        public long Votes { get; set; }

        public string Description { get; set; }

        public List<PackageVersionDTO> Versions { get; set; }
    }
}
