using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Consul;
using GalaFamilyLibrary.Infrastructure.Cors;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
services.AddDbSetup();
services.AddScoped(typeof(IPackageService), typeof(PackageService));
services.AddGenericSetup(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGeneric();