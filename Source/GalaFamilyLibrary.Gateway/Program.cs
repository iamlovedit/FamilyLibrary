using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using Microsoft.AspNetCore.Http;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net;
using System.Net.Http;
using GalaFamilyLibrary.Gateway.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((builderContext, builder) =>
{
    builder.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddJsonFile("ocelot.json", false, true)
        .AddJsonFile($"ocelot.{builderContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();
});
var services = builder.Services;
services.AddRedisCacheSetup(builder.Configuration);
services.AddLogging(config =>
{
    config.AddConsole();
    config.AddFilter(null, LogLevel.Warning);
});
services.AddJwtAuthentication(builder.Configuration);

services.AddOcelot() /*.AddConsul()*/;
var app = builder.Build();
app.UseAuthentication();
// app.UseMiddleware<ResponseMiddleware>();
app.UseOcelot().ConfigureAwait(true);
app.Run();