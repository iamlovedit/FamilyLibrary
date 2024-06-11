using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
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
services.AddRedisCacheSetup(builder.Configuration);

services.AddSeqSetup(builder.Configuration);

services.AddAuthorizationSetup(builder.Configuration);
services.AddJwtAuthenticationSetup(builder.Configuration);
services.AddOcelot().AddPolly();
var app = builder.Build();

app.UseAuthentication();
await app.UseOcelot().ConfigureAwait(true);
app.Run();