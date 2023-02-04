using GalaFamilyLibrary.FamilyService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.FamilyService.Controllers.v1;

[Route("api/v{version:apiVersion}/family")]
[ApiVersion("1.0")]
public class FamilyController:ApiControllerBase
{
    private readonly IFamilyService _familyService;
    private readonly ILogger<FamilyController> _logger;

    public FamilyController(IFamilyService familyService,ILogger<FamilyController> logger)
    {
        _familyService = familyService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<string> Get()
    {
        return await Task.FromResult(nameof(FamilyController));
    }
}