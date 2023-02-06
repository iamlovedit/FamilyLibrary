using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", false, true);
var services = builder.Services;

services.AddOcelot()/*.AddConsul()*/;
services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
app.UseAuthentication().UseOcelot().Wait();
app.Run();