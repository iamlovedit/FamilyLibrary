using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.Cors;

public static class CorsSetup
{
    private static readonly string _corsSchema = "CorsPolicy";

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

    public static void UseCorsService(this IApplicationBuilder app)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));
        app.UseCors(_corsSchema);
    }
}