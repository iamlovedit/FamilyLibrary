using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddScoped(typeof(IPackageService), typeof(PackageService));
builder.AddGenericSetup();

services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTables(typeof(Program));
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }
    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Packages");
    dbSeed.InitSeed<DynamoPackage>(file);

    file = string.Format(seedFolder, "PackageVersions");
    dbSeed.InitSeed<PackageVersion>(file);
});
app.UseGeneric();