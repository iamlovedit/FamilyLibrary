using Newtonsoft.Json;

namespace GalaFamilyLibrary.Model.Package
{
    public class PublishedVersion
    {
        [JsonProperty("full_dependency_versions")]
        public List<string> FullDependencyVersions { get; set; }

        public string Size { get; set; }

        public string Version { get; set; }

        [JsonProperty("scan_status")] public string ScanStatus { get; set; }

        [JsonProperty("node_libraries")] public List<string> NodeLibraries { get; set; }

        public string Contents { get; set; }

        public DateTime Created { get; set; }

        [JsonProperty("contains_binaries")] public bool ContainsBinaries { get; set; }

        public string Url { get; set; }

        [JsonProperty("engine_version")] public string EngineVersion { get; set; }

        [JsonProperty("direct_dependency_versions")]
        public List<string> DirectDependencyVersions { get; set; }

        [JsonProperty("full_dependency_ids")] public List<Dependency> FullDependencyIds { get; set; }

        [JsonProperty("direct_dependency_ids")]
        public List<Dependency> DirectDependencyIds { get; set; }
    }
}