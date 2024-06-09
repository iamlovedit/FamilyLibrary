using Microsoft.AspNetCore.Builder;

namespace GalaFamilyLibrary.Infrastructure.Cors
{
    public static class CorsMiddleware
    {
        public static void UseCorsService(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            app.UseCors(CorsSetup._corsSchema);
        }
    }
}

