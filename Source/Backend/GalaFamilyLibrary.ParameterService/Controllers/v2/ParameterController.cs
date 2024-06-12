using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.ParameterService.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("parameter/v{version:apiVersion}")]
    public class ParameterController
    {
        public ParameterController()
        {
        }
    }
}