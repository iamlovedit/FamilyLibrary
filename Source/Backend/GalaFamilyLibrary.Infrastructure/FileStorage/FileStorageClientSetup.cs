﻿namespace GalaFamilyLibrary.Infrastructure.FileStorage
{
    public static class FileStorageClientSetup
    {
        public static void AddFileStorageClientSetup(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(configuration);
            //var config = configuration.GetSection(nameof(FileStorageClient));
            //services.Configure<FileStorageClient>(config);
            services.AddSingleton<FileStorageClient>();
        }

        public static void AddFileSecurityOptionSetup(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(configuration);
            //var config = configuration.GetSection(nameof(FileSecurityOption));
            //services.Configure<FileSecurityOption>(config);
            services.AddSingleton<FileSecurityOption>();
        }
    }
}
