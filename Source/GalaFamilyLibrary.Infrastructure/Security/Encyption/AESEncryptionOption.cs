using Microsoft.Extensions.Configuration;

namespace GalaFamilyLibrary.Infrastructure.Security.Encyption
{
    public class AESEncryptionOption
    {
        public string? SecretParameterEncryptKey { get; set; }

        public string? SecretParameterIV { get; set; }

        public AESEncryptionOption(IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(AESEncryptionOption));
            SecretParameterEncryptKey = section[nameof(SecretParameterEncryptKey)];
            SecretParameterIV = section[nameof(SecretParameterIV)];
        }
    }
}
