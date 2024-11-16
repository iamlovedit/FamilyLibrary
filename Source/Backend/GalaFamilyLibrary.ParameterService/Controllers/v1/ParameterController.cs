using Asp.Versioning;
using GalaFamilyLibrary.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.ParameterService.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("parameter/v{version:apiVersion}")]
    public class ParameterController : DefaultControllerBase
    {
    }
}