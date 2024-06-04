using GalaFamilyLibrary.Domain.Models.FamilyParameter;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.ParameterService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddGenericSetup();
var services = builder.Services;
services.AddScoped(typeof(IParameterService), typeof(ParameterService));
services.AddScoped(typeof(IParameterDefinitionService), typeof(ParameterDefinitionService));
services.AddScoped(typeof(IParameterGroupService), typeof(ParameterGroupService));
services.AddScoped(typeof(IParameterTypeService), typeof(ParameterTypeService));
services.AddScoped(typeof(IParameterUnitTypeService), typeof(ParameterUnitTypeService));
var app = builder.Build();

app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTablesByClass(typeof(Parameter));

    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }
    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");

    var file = string.Format(seedFolder, "FamilyParameters");
    dbSeed.InitSeed<Parameter>(file);

    file = string.Format(seedFolder, "ParameterDefinitions");
    dbSeed.InitSeed<ParameterDefinition>(file);

    file = string.Format(seedFolder, "ParameterGroups");
    dbSeed.InitSeed<ParameterGroup>(file);

    file = string.Format(seedFolder, "ParameterTypes");
    dbSeed.InitSeed<ParameterType>(file);

    file = string.Format(seedFolder, "ParameterUnitTypes");
    dbSeed.InitSeed<ParameterUnitType>(file);

    file = string.Format(seedFolder, "DisplayUnitTypes");
    dbSeed.InitSeed<DisplayUnitType>(file);
});
app.UseGeneric();
