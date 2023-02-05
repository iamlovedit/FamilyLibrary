using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.Consul;

public static class ConsulConfigSetup
{
    public static void AddConsulConfigSetup(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration.GetValue<string>("Consul:Host");
            consulConfig.Address = new Uri(address);
        }));
    }
}