using GalaFamilyLibrary.DynamoPackageService.Models;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FetchPackagesJob> _logger;

        public FetchPackagesJob(AppDbContext appDbContext, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, ILogger<FetchPackagesJob> logger)
        {
            _appDbContext = appDbContext;
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var responseMessage = await httpClient.GetAsync("https://dynamopackages.com/packages");
            _logger.LogInformation("start a task for update dynamo packages");
            if (responseMessage.IsSuccessStatusCode)
            {
                //TODO:update database
                var json = await responseMessage?.Content?.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        var jObject = JObject.Parse(json);
                        var content = jObject["content"];
                        if (!string.IsNullOrEmpty(content?.ToString()))
                        {
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
                                    var oldPackageVersion = oldPackageVersions.FirstOrDefault(v => v.Id == pVersion.Id);
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
                            _logger.LogInformation("added new package count {added},added new version count {addedverson}",addedPackages.Count,addedPackageVersions.Count);
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
                                var newVersion = newPackageVersions.FirstOrDefault(v => v.Id == pVersion.Id);
                                if (newVersion is null)
                                {
                                    pVersion.IsDeleted = true;
                                    await packageVersionDb.UpdateAsync(pVersion);
                                }
                            }
                            _unitOfWork.CommitTransaction();
                        }
                    }
                    catch (Exception e)
                    {
                        _unitOfWork.RollbackTransaction();
                        _logger.LogError(e, e.Message);
                    }
                }
            }
        }
    }
}
