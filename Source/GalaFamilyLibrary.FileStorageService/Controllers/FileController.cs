using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace GalaFamilyLibrary.FileStorageService.Controllers;

[ApiController]
[Route("files")]
public class FileController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly IAESEncryptionService _aESEncryptionService;
    private readonly FileSecurityOption _fileSecurityOption;
    public FileController(IWebHostEnvironment webHostEnvironment, IAESEncryptionService aESEncryptionService, FileSecurityOption fileSecurityOption)
    {
        _environment = webHostEnvironment;
        _aESEncryptionService = aESEncryptionService;
        _fileSecurityOption = fileSecurityOption;
    }

    [HttpGet]
    [Route("{*path}")]
    public Task<IActionResult> Download(string path, string token)
    {
        return Task.Run<IActionResult>(() =>
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            var unprotectionToken = _aESEncryptionService.Decrypt(token);
            var options = FileSecurityOption.GetOption(unprotectionToken);
            if (options is null)
            {
                return BadRequest();
            }
            if (options.AccessKey == _fileSecurityOption.AccessKey && options.Expiration < DateTime.Now)
            {
                return BadRequest();
            }
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