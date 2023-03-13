using AutoMapper;
using GalaFamilyLibrary.DynamoPackageService.Helpers;
using GalaFamilyLibrary.DynamoPackageService.Jobs;
using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddScoped(typeof(IPackageService), typeof(PackageService));
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.AddGenericSetup();
services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("update packages");
    options.AddJob<FetchPackagesJob>(config => config.WithIdentity(jobKey));
    options.AddTrigger(config =>
    {
        config.ForJob(jobKey)
            .WithIdentity("update packages")
            .WithCronSchedule("0 0 0 1/1 * ? *");
    });
});
services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
services.AddSingleton(provider => new MapperConfiguration(config =>
{
    var profile = new MappingProfiles(provider.GetService<IAESEncryptionService>());
    config.AddProfile(profile);
}).CreateMapper());

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