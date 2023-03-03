using GalaFamilyLibrary.Infrastructure.Common;

namespace GalaFamilyLibrary.Infrastructure.FileStorage
{
    public class FileStorageClient
    {
        private readonly DataProtectionHelper _dataProtectionHelper;

        public string Endpoint { get; set; }

        public string Bucket { get; set; }

        public FileStorageClient(DataProtectionHelper dataProtectionHelper)
        {
            _dataProtectionHelper = dataProtectionHelper;
        }

        public string GetCreateUploadUrl(string path)
        {
            return string.Empty;
        }

        public string GetDownloadUrl(string path)
        {
            return string.Empty;
        }
    }
}
