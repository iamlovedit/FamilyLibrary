using GalaFamilyLibrary.Domain.Models.Dynamo;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Repository;
using GalaFamilyLibrary.Infrastructure.Seed;
using Newtonsoft.Json.Linq;
using Quartz;

namespace GalaFamilyLibrary.DynamoPackageService.Jobs
{
    public class FetchPackagesJob(
        DatabaseContext databaseContext,
        IUnitOfWork unitOfWork,
        IRedisBasketRepository redis,
        IHttpClientFactory httpClientFactory,
        ILogger<FetchPackagesJob> logger)
        : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var httpClient = httpClientFactory.CreateClient();
            var responseMessage = await httpClient.GetAsync("https://dynamopackages.com/packages");
            if (responseMessage.IsSuccessStatusCode)
            {
                var json = await responseMessage?.Content?.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        var jObject = JObject.Parse(json);
                        var content = jObject["content"];
                        if (content is null)
                        {
                            return;
                        }

                        var newPackages = content.ToObject<List<DynamoPackage>>();
                        var packageDb = databaseContext.GetEntityDatabase<DynamoPackage>();
                        var packageVersionDb = databaseContext.GetEntityDatabase<PackageVersion>();
                        var oldPackages = await packageDb.GetListAsync();
                        var oldPackageVersions = await packageVersionDb.GetListAsync();
                        unitOfWork.BeginTransaction();
                        var addedPackages = new List<DynamoPackage>();
                        var addedPackageVersions = new List<PackageVersion>();
                        var newPackageVersions = new List<PackageVersion>();
                        foreach (var package in newPackages)
                        {
                            var oldPackage = oldPackages.FirstOrDefault(p => p.Id == package.Id);
                            if (oldPackage is null)
                            {
                                addedPackages.Add(package);
                            }
                            else
                            {
                                await packageDb.UpdateAsync(package);
                            }

                            foreach (var pVersion in package.Versions)
                            {
                                pVersion.PackageId = package.Id;
                                var oldPackageVersion = oldPackageVersions.FirstOrDefault(pv =>
                                    pv.PackageId == package.Id && pv.Version == pVersion.Version);
                                if (oldPackageVersion is null)
                                {
                                    addedPackageVersions.Add(pVersion);
                                }
                                else
                                {
                                    await packageVersionDb.UpdateAsync(pVersion);
                                }

                                newPackageVersions.Add(pVersion);
                            }
                        }

                        await packageDb.InsertRangeAsync(addedPackages);
                        await packageVersionDb.InsertRangeAsync(addedPackageVersions);

                        foreach (var package in oldPackages)
                        {
                            var newPackage = newPackages.FirstOrDefault(p => p.Id == package.Id);
                            if (newPackage is null)
                            {
                                package.IsDeleted = true;
                                await packageDb.UpdateAsync(package);
                            }
                        }

                        foreach (var pVersion in oldPackageVersions)
                        {
                            var newVersion = newPackageVersions.FirstOrDefault(pv =>
                                pv.PackageId == pVersion.PackageId && pv.Version == pVersion.Version);
                            if (newVersion is null)
                            {
                                pVersion.IsDeleted = true;
                                await packageVersionDb.UpdateAsync(pVersion);
                            }
                        }

                        logger.LogInformation(
                            "update succeed,added new package count {added},added new version count {addedverson}",
                            addedPackages.Count, addedPackageVersions.Count);
                        unitOfWork.CommitTransaction();
                        await redis.Clear();
                    }
                    catch (Exception e)
                    {
                        unitOfWork.RollbackTransaction();
                        logger.LogError(e, e.Message);
                    }
                }
            }
            else
            {
                logger.LogError("request error,http status code : {code}", responseMessage.StatusCode);
            }
        }
    }
}