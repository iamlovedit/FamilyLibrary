using System.Security.Policy;

namespace GalaFamilyLibrary.DataTransferObject.Package
{
    public class PublishedPackageBasicDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int Downloads { get; set; }

        public int Votes { get; set; }

        public string Description { get; set; }

        public string Engine { get; set; }

        public string Group { get; set; }

        public int VersionCount { get; set; }
    }
}