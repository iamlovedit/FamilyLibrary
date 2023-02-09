using GalaFamilyLibrary.Gateway.Middlewares;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using Ocelot.DependencyInjection;
using Ocelot.DownstreamRouteFinder.Middleware;
using Ocelot.DownstreamUrlCreator.Middleware;
using Ocelot.Errors.Middleware;
using Ocelot.LoadBalancer.Middleware;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using Ocelot.Provider.Consul;
using Ocelot.Request.Middleware;
using Ocelot.Requester.Middleware;
using Ocelot.RequestId.Middleware;
using Ocelot.Responder.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((builderContext, builder) =>
{
    builder.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", true, true)
    .AddJsonFile("ocelot.json", false, true)
    .AddJsonFile($"ocelot.{builderContext.HostingEnvironment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();
}).ConfigureLogging((context, builder) =>
{
    builder.AddConsole();
    builder.AddFilter(null, LogLevel.Warning);
});
var services = builder.Services;
services.AddOcelot()/*.AddConsul()*/;
services.AddLogging();
services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
app.UseAuthentication();
app.UseOcelot().ConfigureAwait(true);
app.Run();