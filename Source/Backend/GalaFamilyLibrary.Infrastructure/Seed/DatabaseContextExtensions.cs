namespace GalaFamilyLibrary.Infrastructure.Seed;

public static class DatabaseContextExtensions
{
    public static void UseInitSeed(this IApplicationBuilder app, Action<DatabaseSeed> seedBuilder)
    {
        ArgumentNullException.ThrowIfNull(app);

        ArgumentNullException.ThrowIfNull(seedBuilder);

        using var scope = app.ApplicationServices.CreateScope();
        var databaseSeed = scope.ServiceProvider.GetRequiredService<DatabaseSeed>();
        seedBuilder.Invoke(databaseSeed);
    }
    
    public static void GenerateTablesByClass<T>(this IServiceProvider services,
        Action<DatabaseSeed>? seedBuilder = null) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(services);
        var databaseSeed = services.GetRequiredService<DatabaseSeed>()
                           ?? throw new ArgumentNullException($"{nameof(DatabaseSeed)} not found");
        databaseSeed.GenerateTablesByClass<T>();
        seedBuilder?.Invoke(databaseSeed);
    }
}