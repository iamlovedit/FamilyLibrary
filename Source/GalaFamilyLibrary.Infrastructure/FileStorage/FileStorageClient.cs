using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.Extensions.Configuration;

namespace GalaFamilyLibrary.Infrastructure.FileStorage
{
    public class FileStorageClient
    {
        private readonly IAESEncryptionService _aesEncryptionService;
        private readonly FileSecurityOption _securityOption;
        public FileStorageClient(IConfiguration configuration, IAESEncryptionService aesEncryptionService, FileSecurityOption fileSecurityOption)
        {
            _aesEncryptionService = aesEncryptionService;
            _securityOption = fileSecurityOption;
            var section = configuration.GetSection(nameof(FileStorageClient));
            Endpoint = section[nameof(Endpoint)];
            Bucket = section[nameof(Bucket)];
        }
        public string Endpoint { get; }

        public string Bucket { get; }

        public string GetFileUrl(string path)
        {
            var url = Path.Combine(Endpoint, Bucket, path);
            var token = _securityOption.GetAccessToken();
            var protectedToken = _aesEncryptionService.Encrypt(token);
            return url += $"?token={protectedToken}";
        }
    }
}
