using GalaFamilyLibrary.Infrastructure.Domains;
using Newtonsoft.Json;

namespace GalaFamilyLibrary.Model.Package
{
    public class PublishedPackage : IdentifiableBase<string>
    {
        public DateTime Created { get; set; }

        public int Downloads { get; set; }

        public int Votes { get; set; }

        [JsonProperty("latest_version_update")]
        public DateTime Updated { get; set; }

        public List<Publisher> Maintainers { get; set; }

        public List<string> Keywords { get; set; }

        public string Description { get; set; }

        public string Engine { get; set; }

        public string Group { get; set; }

        public List<PublishedVersion> Versions { get; set; }

        [JsonProperty("used_by")] public List<Dependency> UsedBy { get; set; }

        [JsonProperty("num_versions")] public int VersionCount { get; set; }
    }
}