using GalaFamilyLibrary.DynamoPackageService.Jobs;
using GalaFamilyLibrary.Infrastructure.Extensions;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Model.Package;
using GalaFamilyLibrary.Service.Package;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
services.Configure<KestrelServerOptions>(options => { options.Limits.MaxRequestBodySize = long.MaxValue; });

services.AddScoped<IPackageService, PackageService>();
services.AddScoped<IVersionService, VersionService>();
services.AddScoped<IPublishedPackageService, PublishedPackageService>();

builder.AddDefaultInfrastructure<long>();
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
    dbSeed.GenerateTablesByClass<DynamoPackage>();
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }

    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Packages");
    dbSeed.GenerateSeedAsync<DynamoPackage>(file);

    file = string.Format(seedFolder, "PackageVersions");
    dbSeed.GenerateSeedAsync<PackageVersion>(file);
});
app.UseInfrastructure();