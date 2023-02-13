using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.Cors;

public static class CorsSetup
{
    public static readonly string _corsSchema = "CorsPolicy";

    public static void AddCorsSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        services.AddCors(options =>
        {
            options.AddPolicy(_corsSchema, policy =>
            {
                policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
            });
        });
    }
}