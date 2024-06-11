using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider versionDescriptionProvider)
        : IConfigureNamedOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var apiInfo = new OpenApiInfo()
            {
                Title = "gala library",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                apiInfo.Description +=
                    " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
            }

            return apiInfo;
        }
    }
}