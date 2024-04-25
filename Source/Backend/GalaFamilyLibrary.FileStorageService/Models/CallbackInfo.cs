namespace GalaFamilyLibrary.FileStorageService.Models
{
    public class CallbackInfo
    {
        public string CallbackUrl { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public ushort Version { get; set; }
        public int UploaderId { get; set; }
        public string MD5 { get; set; }
        public string FileId { get; set; }
    }
}
