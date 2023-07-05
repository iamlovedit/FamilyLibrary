using AutoMapper;
using GalaFamilyLibrary.Domain.Models.FamilyLibrary;
using GalaFamilyLibrary.FamilyService.Helpers;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
services.AddFileSecurityOptionSetup(builder.Configuration);
services.AddFileStorageClientSetup(builder.Configuration);

services.AddScoped(typeof(IFamilyService), typeof(FamilyService));
services.AddScoped(typeof(IFamilyCategoryService), typeof(FamilyCategoryService));
builder.AddGenericSetup();
services.AddSingleton(provider => new MapperConfiguration(config =>
{
    config.AddProfile(new MappingProfiles(provider.GetService<FileStorageClient>()));
}).CreateMapper());

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTablesByClass(typeof(Family));
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }
    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Families");
    dbSeed.InitSeed<Family>(file);

    file = string.Format(seedFolder, "FamilySymbols");
    dbSeed.InitSeed<FamilySymbol>(file);

    file = string.Format(seedFolder, "FamilyCategories");
    dbSeed.InitSeed<FamilyCategory>(file);

});
app.UseStaticFiles();
app.UseGeneric();