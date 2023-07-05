using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using AutoMapper;
using GalaFamilyLibrary.FamilyService.Helpers;
using GalaFamilyLibrary.Domain.Models.FamilyLibrary;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
services.AddFileSecurityOptionSetup(builder.Configuration);
services.AddFileStorageClientSetup(builder.Configuration);

services.AddScoped(typeof(IFamilyService), typeof(FamilyService));
services.AddScoped(typeof(IFamilyCategoryService), typeof(FamilyCategoryService));
services.AddScoped(typeof(IFamilyCollectionService), typeof(FamilyCollectionService));
services.AddScoped(typeof(IFamilyStarService), typeof(FamilyStarService));

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

    //file = string.Format(seedFolder, "FamilyCollections");
    //dbSeed.InitSeed<FamilyCollection>(file);

    //file = string.Format(seedFolder, "FamilyStars");
    //dbSeed.InitSeed<FamilyStar>(file);
});
app.UseStaticFiles();
app.UseGeneric();