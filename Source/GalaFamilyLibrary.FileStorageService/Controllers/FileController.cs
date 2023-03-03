using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace GalaFamilyLibrary.FileStorageService.Controllers;

[ApiController]
[Route("files")]
public class FileController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly DataProtectionHelper _dataProtectionHelper;

    public FileController(IWebHostEnvironment webHostEnvironment, DataProtectionHelper dataProtectionHelper)
    {
        _environment = webHostEnvironment;
        _dataProtectionHelper = dataProtectionHelper;
    }

    [HttpGet]
    [Route("{*path}")]
    public Task<IActionResult> Download(string path, string token)
    {
        return Task.Run<IActionResult>(() =>
        {
            if (string.IsNullOrEmpty(token))
            {
                return NotFound();
            }
            //var unprotectionToken = _dataProtectionHelper.Decrypt(token, "fileKey");
            var filePath = Path.Combine(_environment.ContentRootPath, "Files", path);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filePath, contentType);
        });
    }

    [HttpPost]
    [Route("{*path}")]
    public Task<IActionResult> Upload(string path, string token, IFormFile file)
    {
        return Task.Run<IActionResult>(() =>
        {
            //var unprotectionToken = _dataProtectionHelper.Decrypt(token, "fileKey");
            if (string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            return Ok();
        });
    }
}