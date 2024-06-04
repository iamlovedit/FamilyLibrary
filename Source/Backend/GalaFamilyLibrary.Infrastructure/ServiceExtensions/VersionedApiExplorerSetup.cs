using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class VersionedApiExplorerSetup
{
    public static void AddVersionedApiExplorerSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddVersionedApiExplorer(builder =>
        {
            builder.GroupNameFormat = "'v'VVV";
            builder.SubstituteApiVersionInUrl = true;
        });
        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }
}