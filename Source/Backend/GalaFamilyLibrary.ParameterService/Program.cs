using GalaFamilyLibrary.Infrastructure.Extensions;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Model.FamilyParameter;
using GalaFamilyLibrary.Service.FamilyParameter;

var builder = WebApplication.CreateBuilder(args);
builder.AddDefaultInfrastructure<long>();
var services = builder.Services;
services.AddScoped<IParameterService, ParameterService>();
services.AddScoped<IParameterDefinitionService, ParameterDefinitionService>();
var app = builder.Build();

app.UseInitSeed(dbSeed =>
{
    dbSeed.GenerateTablesByClass<Parameter>();

    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }

    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");

    var file = string.Format(seedFolder, "FamilyParameters");
    dbSeed.GenerateSeedAsync<Parameter>(file);

    file = string.Format(seedFolder, "ParameterDefinitions");
    dbSeed.GenerateSeedAsync<ParameterDefinition>(file);

    file = string.Format(seedFolder, "ParameterGroups");
    dbSeed.GenerateSeedAsync<ParameterGroup>(file);

    file = string.Format(seedFolder, "ParameterTypes");
    dbSeed.GenerateSeedAsync<ParameterType>(file);

    file = string.Format(seedFolder, "ParameterUnitTypes");
    dbSeed.GenerateSeedAsync<ParameterUnitType>(file);

    file = string.Format(seedFolder, "DisplayUnitTypes");
    dbSeed.GenerateSeedAsync<DisplayUnitType>(file);
});
app.UseInfrastructure();