using GalaFamilyLibrary.DynamoPackageService.Jobs;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.Model.Package;
using GalaFamilyLibrary.Service.Package;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Polly;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});

services.AddScoped(typeof(IPackageService), typeof(PackageService));
services.AddScoped(typeof(IVersionService), typeof(VersionService));

builder.AddInfrastructureSetup();
services.AddQuartz(options =>
{
    var jobKey = new JobKey("update packages");
    options.AddJob<FetchPackagesJob>(config => config.WithIdentity(jobKey));
    options.AddTrigger(config =>
    {
        config.ForJob(jobKey)
            .WithIdentity("update packages")
            .WithCronSchedule("0 0 5 1/1 * ? *");
    });
});
services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTablesByClass<DynamoPackage>();
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
app.UseInfrastructure();