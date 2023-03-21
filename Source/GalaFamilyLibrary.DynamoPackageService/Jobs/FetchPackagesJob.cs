using GalaFamilyLibrary.DynamoPackageService.Models;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.Transaction;
using Newtonsoft.Json.Linq;
using Quartz;

namespace GalaFamilyLibrary.DynamoPackageService.Jobs
{
    public class FetchPackagesJob : IJob
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisBasketRepository _redis;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FetchPackagesJob> _logger;

        public FetchPackagesJob(AppDbContext appDbContext, IUnitOfWork unitOfWork, IRedisBasketRepository redis,
            IHttpClientFactory httpClientFactory, ILogger<FetchPackagesJob> logger)
        {
            _appDbContext = appDbContext;
            _unitOfWork = unitOfWork;
            _redis = redis;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var httpClient = _httpClientFactory.CreateClient();
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
                        var packageDb = _appDbContext.GetEntityDB<DynamoPackage>();
                        var packageVersionDb = _appDbContext.GetEntityDB<PackageVersion>();
                        var oldPackages = await packageDb.GetListAsync();
                        var oldPackageVersions = await packageVersionDb.GetListAsync();
                        _unitOfWork.BeginTransaction();
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

                        _logger.LogInformation(
                            "update succeed,added new package count {added},added new version count {addedverson}",
                            addedPackages.Count, addedPackageVersions.Count);
                        _unitOfWork.CommitTransaction();
                        await _redis.Clear();
                    }
                    catch (Exception e)
                    {
                        _unitOfWork.RollbackTransaction();
                        _logger.LogError(e, e.Message);
                    }
                }
            }
            else
            {
                _logger.LogError("request error,http status code : {code}", responseMessage.StatusCode);
            }
        }
    }
}