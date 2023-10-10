using AutoMapper;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.AutoMapper
{
    public static class AutoMapperExtension
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile(provider.GetService<FileStorageClient>()));
            }).CreateMapper());
        }
    }
}
