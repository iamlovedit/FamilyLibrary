using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
services.AddScoped(typeof(IFamilyService), typeof(FamilyService));

services.AddDbSetup();
services.AddGenericSetup(builder.Configuration);

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseGeneric();