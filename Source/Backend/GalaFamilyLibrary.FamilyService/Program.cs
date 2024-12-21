using FluentValidation;
using GalaFamilyLibrary.DataTransferObject.FamilyLibrary;
using GalaFamilyLibrary.Infrastructure.Extensions;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Model.FamilyLibrary;
using GalaFamilyLibrary.Service.FamilyLibrary;
using GalaFamilyLibrary.Service.Validators;
using Minio;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

services.AddScoped<IFamilyLikeCountService, FamilyLikeCountService>();
services.AddScoped<IFamilyService, FamilyService>();
services.AddScoped<IFamilyLikesService, FamilyLikesService>();
services.AddScoped<IFamilyVersionService, FamilyVersionService>();
services.AddScoped<IFamilyCollectionsService, FamilyCollectionsService>();
services.AddScoped<IValidator<FamilyCreationDTO>, FamilyCreationValidator>();
builder.AddDefaultInfrastructure<long>();

services.AddMinio(client =>
{
    client.WithEndpoint(configuration["MINIO_HOST"])
        .WithCredentials(configuration["MINIO_ROOT_USER"], configuration["MINIO_ROOT_PASSWORD"]);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseInitSeed(dbSeed =>
{
    dbSeed.GenerateTablesByClass<Family>();
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }

    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Families");
    dbSeed.GenerateSeedAsync<Family>(file);

    file = string.Format(seedFolder, "FamilySymbols");
    dbSeed.GenerateSeedAsync<FamilySymbol>(file);

    file = string.Format(seedFolder, "FamilyCategories");
    dbSeed.GenerateSeedAsync<FamilyCategory>(file);

    file = string.Format(seedFolder, "FamilyDetails");
    dbSeed.GenerateSeedAsync<FamilyDetail>(file);
});
app.UseStaticFiles();
app.UseDefaultInfrastructure();