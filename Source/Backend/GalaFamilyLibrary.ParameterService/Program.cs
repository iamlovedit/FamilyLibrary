using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.Model.FamilyParameter;
using GalaFamilyLibrary.Service.FamilyParameter;

var builder = WebApplication.CreateBuilder(args);
builder.AddInfrastructureSetup();
var services = builder.Services;
services.AddScoped(typeof(IParameterService), typeof(ParameterService));
services.AddScoped(typeof(IParameterDefinitionService), typeof(ParameterDefinitionService));
var app = builder.Build();

app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTablesByClass<Parameter>();

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
app.UseInfrastructure();