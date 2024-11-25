namespace GalaFamilyLibrary.DataTransferObject.Package
{
    public class PublishedVersionDto
    {
        public List<string> FullDependencyVersions { get; set; }

        public string Size { get; set; }

        public string Version { get; set; }

        public string ScanStatus { get; set; }

        public List<string> NodeLibraries { get; set; }

        public string Contents { get; set; }

        public DateTime Created { get; set; }

        public bool ContainsBinaries { get; set; }

        public string Url { get; set; }

        public string EngineVersion { get; set; }

        public List<string> DirectDependencyVersions { get; set; }

        public List<string> FullDependencyIds { get; set; }

        public List<string> DirectDependencyIds { get; set; }
    }
}