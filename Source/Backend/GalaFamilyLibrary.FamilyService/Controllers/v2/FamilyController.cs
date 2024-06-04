using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.FamilyService.Controllers.v2;

[ApiVersion("2.0")]
[Route("family/v{version:apiVersion}")]
public class FamilyController : ApiControllerBase
{

}