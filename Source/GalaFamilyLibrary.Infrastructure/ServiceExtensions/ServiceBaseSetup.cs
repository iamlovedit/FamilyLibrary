using GalaFamilyLibrary.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class ServiceBaseSetup
{
    public static void AddRepositorySetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
    }
}