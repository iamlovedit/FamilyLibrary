using GalaFamilyLibrary.FileStorageService.Models;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System.Net;

namespace GalaFamilyLibrary.FileStorageService.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase
    {
        private readonly IAESEncryptionService _aESEncryptionService;
        private readonly ILogger<FileController> _logger;
        private readonly FileSecurityOption _fileSecurityOption;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _fileFolder;

        public FileController(IWebHostEnvironment webHostEnvironment, IAESEncryptionService aESEncryptionService,
            ILogger<FileController> logger, FileSecurityOption fileSecurityOption, IHttpClientFactory httpClientFactory)
        {
            _aESEncryptionService = aESEncryptionService;
            _logger = logger;
            _fileSecurityOption = fileSecurityOption;
            _httpClientFactory = httpClientFactory;
            _fileFolder = Path.Combine(webHostEnvironment.ContentRootPath, "files");
        }

        [HttpGet]
        [Route("{*path:file}")]
        public async Task<IActionResult> Download(string path, string token)
        {
            return await Task.Run<IActionResult>(() =>
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                var unprotectionToken = _aESEncryptionService.Decrypt(token);
                if (string.IsNullOrEmpty(unprotectionToken))
                {
                    return Unauthorized();
                }

                try
                {
                    var options = JsonConvert.DeserializeObject<FileSecurityOption>(unprotectionToken);
                    if (options is null)
                    {
                        return Unauthorized();
                    }

                    if (options.AccessKey == _fileSecurityOption.AccessKey && options.Expiration < DateTime.Now)
                    {
                        _logger.LogWarning("download file {filename} failed,access denied", options.Filename);
                        return Unauthorized();
                    }

                    var filePath = Path.Combine(_fileFolder, path);
                    if (!System.IO.File.Exists(filePath))
                    {
                        _logger.LogWarning("download file {filename} failed,file not exist", options.Filename);
                        return NotFound("file not exist");
                    }

                    var provider = new FileExtensionContentTypeProvider();
                    if (!provider.TryGetContentType(filePath, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }

                    _logger.LogInformation("download file {filename} succeed", options.Filename);
                    return PhysicalFile(filePath, contentType, options.Filename);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return BadRequest();
                }
            });
        }

        [HttpPost]
        [Route("{*path:file}")]
        public async Task<IActionResult> Upload(string path, string token, IFormFile file,
            [FromForm] CallbackInfo callback)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension == ".rfa" || fileExtension == ".png")
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                var unprotectionToken = _aESEncryptionService.Decrypt(token);
                if (string.IsNullOrEmpty(unprotectionToken))
                {
                    return Unauthorized();
                }

                try
                {
                    var options = JsonConvert.DeserializeObject<FileSecurityOption>(unprotectionToken);
                    if (options is null)
                    {
                        return Unauthorized();
                    }

                    if (options.AccessKey == _fileSecurityOption.AccessKey || options.Expiration < DateTime.Now)
                    {
                        _logger.LogWarning(
                            "upload file {filename},file id {fileId},file extension {fileExtension} failed,access denied",
                            options.Filename, callback.FileId, fileExtension);
                        return Unauthorized();
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.OpenReadStream().CopyToAsync(memoryStream);
                        var fileBytes = memoryStream.ToArray();
                        if (fileBytes.EncryptMD5() == callback.MD5)
                        {
                            var filePath = Path.Combine(_fileFolder, path);
                            if (fileExtension != ".png")
                            {
                                using (var httpClient = _httpClientFactory.CreateClient())
                                {
                                    var keyValues = new KeyValuePair<string, string>[]
                                    {
                                        new KeyValuePair<string, string>("name", callback.Name),
                                        new KeyValuePair<string, string>("categoryId", callback.CategoryId.ToString()),
                                        new KeyValuePair<string, string>("version", callback.Version.ToString()),
                                        new KeyValuePair<string, string>("uploaderId", callback.UploaderId.ToString()),
                                        new KeyValuePair<string, string>("fileId", callback.FileId),
                                    };
                                    var formContent = new FormUrlEncodedContent(keyValues);
                                    var httpResponse = await httpClient.PostAsync(callback.CallbackUrl, formContent);
                                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);
                                        await memoryStream.FlushAsync();
                                        _logger.LogInformation("upload succeed,file name {filename} file extension {fileExtension}",callback.Name,fileExtension);
                                        return Ok("upload succeed");
                                    }
                                    else
                                    {
                                        return Problem("upload failed");
                                    }
                                }
                            }
                            else
                            {
                                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);
                                await memoryStream.FlushAsync();
                                return Ok("upload succeed");
                            }
                        }
                        else
                        {
                            return Problem();
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return Problem(e.Message);
                }
            }

            return BadRequest("error file format");
        }
    }
}