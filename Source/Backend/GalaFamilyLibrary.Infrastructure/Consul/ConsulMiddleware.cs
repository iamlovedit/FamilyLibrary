using Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

namespace GalaFamilyLibrary.Infrastructure.Consul;

public static class ConsulMiddleware
{
    public static void UseConsul(this IApplicationBuilder app, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(app);
        //var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var consulClient = new ConsulClient(x => x.Address = new Uri("http://localhost:8500"));
        var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Consul");
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        var check = new AgentServiceCheck
        {
            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
            Interval = TimeSpan.FromSeconds(10),
            Timeout = TimeSpan.FromSeconds(5)
        };
        var serviceSection = configuration.GetSection("Service");
        var registration = new AgentServiceRegistration()
        {
            Checks = new[] { check },
            ID = Guid.NewGuid().ToString(),
            Name = serviceSection["Name"],
            Address = serviceSection["Address"],
            Port = serviceSection["Port"].ObjToInt()
        };


        //if (app.Properties["server.Features"] is not FeatureCollection features)
        //{
        //    return;
        //}

        //var addresses = features.Get<IServerAddressesFeature>();
        //var address = addresses.Addresses.First();

        //Console.WriteLine($"address={address}");

        //var uri = new Uri(address);
        //var registration = new AgentServiceRegistration()
        //{
        //    ID = $"MyService-{uri.Port}",
        //    // service name  
        //    Name = "MyService",
        //    Address = $"{uri.Host}",
        //    Port = uri.Port
        //};

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