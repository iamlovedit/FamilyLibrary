using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GalaFamilyLibrary.FileStorageService.Models;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.FileStorage;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GalaFamilyLibrary.FileStorageService.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController(
        IWebHostEnvironment webHostEnvironment,
        IAESEncryptionService aESEncryptionService,
        ILogger<FileController> logger,
        FileSecurityOption fileSecurityOption,
        IHttpClientFactory httpClientFactory)
        : ControllerBase
    {
        private readonly string _fileFolder = Path.Combine(webHostEnvironment.ContentRootPath, "files");

        [HttpGet("{*path:file}")]
        public async Task<IActionResult> Download(string path, string token)
        {
            return await Task.Run<IActionResult>(() =>
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                var unprotectionToken = aESEncryptionService.Decrypt(token);
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

                    if (options.AccessKey == fileSecurityOption.AccessKey && options.Expiration < DateTime.Now)
                    {
                        logger.LogWarning("download file {filename} failed,access denied", options.Filename);
                        return Unauthorized();
                    }

                    var filePath = Path.Combine(_fileFolder, path);
                    if (!System.IO.File.Exists(filePath))
                    {
                        logger.LogWarning("download file {filename} failed,file not exist,current root path {folder},file path {filePath}", options.Filename, _fileFolder, filePath);
                        return NotFound("file not exist");
                    }

                    var provider = new FileExtensionContentTypeProvider();
                    if (!provider.TryGetContentType(filePath, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }

                    logger.LogInformation("download file {filename} succeed current root path {folder},file path {filePath}", options.Filename, _fileFolder, filePath);
                    var extension = Path.GetExtension(path).ToLower();
                    var fileName = $"{options.Filename}{extension}";
                    return PhysicalFile(filePath, contentType,fileName);
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return BadRequest();
                }
            });
        }

        [HttpPost("{*path:file}")]
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

                var unprotectionToken = aESEncryptionService.Decrypt(token);
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

                    if (options.AccessKey == fileSecurityOption.AccessKey || options.Expiration < DateTime.Now)
                    {
                        logger.LogWarning(
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
                                using (var httpClient = httpClientFactory.CreateClient())
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
                                        logger.LogInformation("upload succeed,current root path {folder},file path {filePath}", _fileFolder, filePath);
                                        return Ok("upload succeed");
                                    }
                                    else
                                    {
                                        logger.LogInformation("upload failed,current root path {folder},file path {filePath}", _fileFolder, filePath);
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
                    logger.LogError(e.Message);
                    return Problem(e.Message);
                }
            }

            return BadRequest("error file format");
        }
    }
}