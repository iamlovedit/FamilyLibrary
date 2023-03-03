using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.FileStorage
{
    public static class FileStorageClientSetup
    {
        public static void AddFileStorageClientSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            services.Configure<FileStorageClient>(configuration.GetSection(nameof(FileStorageClient)));
        }
    }
}
