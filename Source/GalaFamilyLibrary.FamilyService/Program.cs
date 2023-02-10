using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
services.AddScoped(typeof(IFamilyService), typeof(FamilyService));
services.AddScoped(typeof(IFamilyCategoryService), typeof(FamilyCategoryService));
services.AddGenericSetup(builder.Configuration);

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
    var file = string.Format(seedFolder, "Families");
    dbSeed.InitSeed<Family>(file);

    file = string.Format(seedFolder, "FamilyCategories");
    dbSeed.InitSeed<FamilyCategory>(file);
});
app.UseGeneric();