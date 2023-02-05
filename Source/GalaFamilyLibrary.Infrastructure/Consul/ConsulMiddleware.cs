using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GalaFamilyLibrary.Infrastructure.Consul;

public static class ConsulMiddleware
{
    public static void UseConsul(this IApplicationBuilder app)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        if (app.Properties["server.Features"] is not FeatureCollection features)
        {
            return;
        }

        var addresses = features.Get<IServerAddressesFeature>();
        var address = addresses.Addresses.First();

        Console.WriteLine($"address={address}");

        var uri = new Uri(address);
        var registration = new AgentServiceRegistration()
        {
            ID = $"MyService-{uri.Port}",
            // service name  
            Name = "MyService",
            Address = $"{uri.Host}",
            Port = uri.Port
        };

        logger.LogInformation("Registering with Consul");
        consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

        lifetime.ApplicationStopping.Register(() =>
        {
            logger.LogInformation("Unregistering from Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        });
    }
}