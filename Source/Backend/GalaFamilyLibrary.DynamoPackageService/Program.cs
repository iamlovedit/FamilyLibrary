using AutoMapper;
using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.DynamoPackageService.Jobs;
using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using Polly;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddScoped(typeof(IPackageService), typeof(PackageService));
services.AddScoped(typeof(IVersionService), typeof(VersionService));

builder.AddInfrastructureSetup();
services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("update packages");
    options.AddJob<FetchPackagesJob>(config => config.WithIdentity(jobKey));
    options.AddTrigger(config =>
    {
        config.ForJob(jobKey)
            .WithIdentity("update packages")
            .WithCronSchedule("0 0 5 1/1 * ? *",
                x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai")));
    });
});
services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

services.AddHttpClient<FetchPackagesJob>().AddTransientHttpErrorPolicy(
    policy => policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt))));
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