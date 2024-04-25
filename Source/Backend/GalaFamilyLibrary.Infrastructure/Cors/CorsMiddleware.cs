using Microsoft.AspNetCore.Builder;

namespace GalaFamilyLibrary.Infrastructure.Cors
{
    public static class CorsMiddleware
    {
        public static void UseCorsService(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.UseCors(CorsSetup._corsSchema);
        }
    }
}

