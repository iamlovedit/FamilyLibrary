using GalaFamilyLibrary.Infrastructure.Seed;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class DbSetup
{
    public static void AddDbSetup(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        services.AddScoped<DbSeed>();
        services.AddScoped<AppDbContext>();
    }
}