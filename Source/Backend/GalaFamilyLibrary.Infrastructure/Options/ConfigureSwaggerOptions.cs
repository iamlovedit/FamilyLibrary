using GalaFamilyLibrary.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GalaFamilyLibrary.Infrastructure.Options;

public class ConfigureSwaggerOptions(
    IApiVersionDescriptionProvider provider,
    IConfiguration configuration)
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        var versionOptions = configuration.GetOptions<VersionOptions>(VersionOptions.SectionName);
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description, versionOptions!.SwaggerTitle));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description, string title)
    {
        var info = new OpenApiInfo()
        {
            Title = title,
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description +=
                " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
        }

        return info;
    }
}