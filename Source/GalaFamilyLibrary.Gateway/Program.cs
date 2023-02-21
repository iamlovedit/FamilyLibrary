using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((builderContext, builder) =>
{
    builder.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddJsonFile($"ocelot.{builderContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();
});
var services = builder.Services;
services.AddRedisCacheSetup(builder.Configuration);

services.AddSeqSetup(builder.Configuration);

builder.AddTraceOutputSetup();
services.AddAuthorizationSetup(builder.Configuration);
services.AddJwtAuthentication(builder.Configuration);
services.AddOcelot();
var app = builder.Build();

app.UseAuthentication();
app.UseOcelot().ConfigureAwait(true);
app.Run();