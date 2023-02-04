using GalaFamilyLibrary.Infrastructure.Seed;
using Microsoft.AspNetCore.Builder;

namespace GalaFamilyLibrary.Infrastructure.Middlewares;

public static class SeedDataMiddleware
{
    public static async void UseSeedDataMiddleware(this IApplicationBuilder app, AppDbContext appDbContext)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));
        if (appDbContext == null) throw new ArgumentNullException(nameof(appDbContext));

    }
}