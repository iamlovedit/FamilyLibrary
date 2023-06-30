using GalaFamilyLibrary.FamilyService.Models;
using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using AutoMapper;
using GalaFamilyLibrary.FamilyService.Helpers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
services.AddFileSecurityOptionSetup(builder.Configuration);
services.AddFileStorageClientSetup(builder.Configuration);

services.AddScoped(typeof(IFamilyService), typeof(FamilyService));
services.AddScoped(typeof(IFamilyCategoryService), typeof(FamilyCategoryService));
services.AddScoped(typeof(IFamilyParameterService), typeof(FamilyParameterService));
services.AddScoped(typeof(IParameterDefinitionService), typeof(ParameterDefinitionService));
services.AddScoped(typeof(IParameterGroupService), typeof(ParameterGroupService));
services.AddScoped(typeof(IParameterTypeService), typeof(ParameterTypeService));
services.AddScoped(typeof(IParameterUnitTypeService), typeof(ParameterUnitTypeService));

builder.AddGenericSetup();
services.AddSingleton(provider => new MapperConfiguration(config =>
{
    config.AddProfile(new MappingProfiles(provider.GetService<FileStorageClient>()));
}).CreateMapper());

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

    file = string.Format(seedFolder, "FamilySymbols");
    dbSeed.InitSeed<FamilySymbol>(file);

    file = string.Format(seedFolder, "FamilyCategories");
    dbSeed.InitSeed<FamilyCategory>(file);

    file = string.Format(seedFolder, "FamilyParameters");
    dbSeed.InitSeed<FamilyParameter>(file);

    file = string.Format(seedFolder, "ParameterDefinitions");
    dbSeed.InitSeed<ParameterDefinition>(file);

    file = string.Format(seedFolder, "ParameterGroups");
    dbSeed.InitSeed<ParameterGroup>(file);

    file = string.Format(seedFolder, "ParameterTypes");
    dbSeed.InitSeed<ParameterType>(file);

    file = string.Format(seedFolder, "ParameterUnitTypes");
    dbSeed.InitSeed<ParameterUnitType>(file);
});
app.UseStaticFiles();
app.UseGeneric();