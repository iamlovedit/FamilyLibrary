using AutoMapper;
using FluentValidation;
using GalaFamilyLibrary.DataTransferObject.FamilyLibrary;
using GalaFamilyLibrary.FamilyService.Helpers;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.Model.FamilyLibrary;
using GalaFamilyLibrary.Service.FamilyLibrary;
using GalaFamilyLibrary.Service.Validators;
using Minio;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;
services.AddFileSecurityOptionSetup(configuration);
services.AddFileStorageClientSetup(configuration);

services.AddScoped(typeof(IFamilyService), typeof(FamilyService));
services.AddScoped<IValidator<FamilyCreationDTO>, FamilyCreationValidator>();
builder.AddInfrastructureSetup();

services.AddMinio(client =>
{
    client.WithEndpoint(configuration["MINIO_HOST"])
        .WithCredentials(configuration["MINIO_ROOT_USER"], configuration["MINIO_ROOT_PASSWORD"]);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTablesByClass<Family>();
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
app.UseInfrastructure();