using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

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
services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
app.UseAuthentication().UseOcelot().Wait();
app.Run();