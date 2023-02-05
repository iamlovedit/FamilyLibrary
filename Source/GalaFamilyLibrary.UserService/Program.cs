using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.UserService.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddScoped(typeof(IUserService), typeof(UserService));
services.AddGenericSetup(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGeneric();
