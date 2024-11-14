using GalaFamilyLibrary.Infrastructure.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", false, false)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, false)
    .AddEnvironmentVariables();
var services = builder.Services;
services.AddDefaultRedis(builder.Configuration);

services.AddDefaultSerilog(builder.Configuration);

services.AddDefaultAuthentication(builder.Configuration);
services.AddDefaultAuthorize(builder.Configuration);
services.AddOcelot().AddPolly();
var app = builder.Build();

app.UseAuthentication();
await app.UseOcelot().ConfigureAwait(true);
app.Run();