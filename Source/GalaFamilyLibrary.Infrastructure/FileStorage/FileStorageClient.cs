using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.Extensions.Configuration;
using System.Web;

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

        /// <remarks>https://stackoverflow.com/questions/5450190/how-to-encode-the-plus-symbol-in-a-url</remarks>
        public string GetFileUrl(string filename,string path)
        {
            var url = Path.Combine(Endpoint, Bucket, path);
            var token = _securityOption.GetAccessToken(filename);
            var protectedToken = _aesEncryptionService.Encrypt(token);
            var values = HttpUtility.ParseQueryString(string.Empty);
            values["token"] = protectedToken;
            var uriBuilder = new UriBuilder(url)
            {
                Query = values.ToString()
            };
            return uriBuilder.ToString();
        }
    }
}
