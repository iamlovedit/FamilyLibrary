using AutoMapper;
using GalaFamilyLibrary.DynamoPackageService.DataTransferObjetcts;
using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
public class PackageController : ApiControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ILogger<PackageController> _logger;
    private readonly IMapper _mapper;

    public PackageController(IPackageService packageService, ILogger<PackageController> logger, IMapper mapper)
    {
        _packageService = packageService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<MessageModel<PackageDTO>> GetPackageAsync(string id)
    {
        var package = await _packageService.GetDynamoPackageByIdAsync(id);
        if (package != null)
        {
            _logger.LogInformation("query packages details by id {id}", id);
            return Success(_mapper.Map<PackageDTO>(package));
        }
        return Failed<PackageDTO>("not found");
    }

    [HttpGet]
    [Route("{id}/{packageVersion}")]
    public async Task<IActionResult> Download(string id, string packageVersion)
    {
        _logger.LogInformation("download package id:{id} version:{packageVersion}", id, packageVersion);
        return await Task.FromResult(Redirect($"https://dynamopackages.com/download/{id}/{packageVersion}"));
    }

    [HttpGet]
    [Route("packages")]
    public async Task<MessageModel<PageModel<PackageDTO>>> GetPackagesByPage(string keyword = "", int pageIndex = 1, int pageSize = 30, string orderField = "")
    {
        _logger.LogInformation("query packages at page:{page} pageSize:{pageSize} keyword:{keyword}", pageIndex, pageSize, keyword);

        var packagesPage = await _packageService.QueryPageAsync(
           string.IsNullOrEmpty(keyword) ? null : p => p.Name.Contains(keyword), pageIndex, pageSize,
           string.IsNullOrEmpty(orderField) ? "" : $"{orderField} desc");
        return SucceedPage(packagesPage.ConvertTo<PackageDTO>(_mapper));
    }
}