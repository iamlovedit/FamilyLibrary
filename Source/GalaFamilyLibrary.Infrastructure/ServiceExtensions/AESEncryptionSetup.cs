using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class AESEncryptionSetup
    {
        public static void AddAESEncryptionSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //services.Configure<AESEncryptionOption>(configuration.GetSection(nameof(AESEncryptionOption)));
            services.AddSingleton<AESEncryptionOption>();
            services.AddSingleton<IAESEncryptionService, AESEncryptionService>();
        }
    }
}
