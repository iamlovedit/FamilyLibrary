using GalaFamilyLibrary.Infrastructure.Seed;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class DbSetup
{
    public static void AddDbSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddScoped<DatabaseSeed>();
        services.AddScoped<DatabaseContext>();
    }
}