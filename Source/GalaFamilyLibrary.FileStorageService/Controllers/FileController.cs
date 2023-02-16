using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace GalaFamilyLibrary.FileStorageService.Controllers;

[Route("files")]
public class FileController : ApiControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public FileController(IWebHostEnvironment webHostEnvironment)
    {
        _environment = webHostEnvironment;
    }

    [HttpGet("{*path:file}")]
    public IActionResult Get(string path)
    {
        var filePath = Path.Combine(_environment.WebRootPath, "Files", path);
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
    }
}