using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public static class DataProtectionSetup
    {
        public static void AddDataProtectionSetup(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddDataProtection().UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });
            services.AddSingleton<IDataProtectionHelper, DataProtectionHelper>();
        }
    }
}
