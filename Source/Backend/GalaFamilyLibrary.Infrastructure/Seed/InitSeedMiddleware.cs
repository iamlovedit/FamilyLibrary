namespace GalaFamilyLibrary.Infrastructure.Seed;

public static class InitSeedMiddleware
{
    public static void UseInitSeed(this IApplicationBuilder app, Action<DatabaseSeed> seedBuilder)
    {
        ArgumentNullException.ThrowIfNull(app);

        ArgumentNullException.ThrowIfNull(seedBuilder);

        using var scope = app.ApplicationServices.CreateScope();
        var databaseSeed = scope.ServiceProvider.GetRequiredService<DatabaseSeed>();
        seedBuilder.Invoke(databaseSeed);
    }
}