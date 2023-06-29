namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class FamilyDetailDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public FamilyCategoryBasicDTO Category { get; set; }

        public List<FamilySymbolDTO> Symbols { get; set; }

        public List<ushort> Versions { get; set; }

        public string FileId { get; set; }

        public int Stars { get; set; }

        public uint Downloads { get; set; }

        public DateTime CreateTime { get; set; }

        public string? Description { get; set; }

        internal string GetImagePath(IWebHostEnvironment environment)
        {
            return Path.Combine(environment.WebRootPath, "images", $"{FileId}.png");
        }
    }
}