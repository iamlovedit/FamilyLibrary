namespace GalaFamilyLibrary.DataTransferObject.Package
{
    public class PublishedPackageDetailDto : PublishedPackageBasicDto
    {
        public List<string> Maintainers { get; set; }

        public List<string> Keywords { get; set; }

        public List<PublishedVersionDto> Versions { get; set; }

        public List<string> UsedBy { get; set; }
    }
}