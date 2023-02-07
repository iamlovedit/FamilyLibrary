﻿using GalaFamilyLibrary.DynamoPackageService.Services;
using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.DynamoPackageService.Controllers.v1;

[ApiVersion("1.0")]
public class PackageController:ApiControllerBase
{
    private readonly IPackageService _packageService;
    private readonly ILogger<PackageController> _logger;

    public PackageController(IPackageService packageService,ILogger<PackageController> logger)
    {
        _packageService = packageService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<string> Get()
    {
        return await Task.FromResult(nameof(PackageController));
    }
    [HttpGet("get1")]
    public async Task<string> Get1()
    {
        return await Task.FromResult("package get1");
    }
}