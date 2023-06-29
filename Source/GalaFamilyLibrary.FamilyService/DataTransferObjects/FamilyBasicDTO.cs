namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class FamilyBasicDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public FamilyCategoryBasicDTO Category { get; set; }

        public string ImageUrl { get; set; }

        public int Stars { get; set; }

        public uint Downloads { get; set; }

        public DateTime CreateTime { get; set; }
    }

}