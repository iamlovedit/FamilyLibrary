using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class ApiAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public ApiAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        var message = JsonConvert.SerializeObject(new ApiResponse(StatusCode.Code401).Message);
        await Response.WriteAsync(message);
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = StatusCodes.Status403Forbidden;
        var message = JsonConvert.SerializeObject(new ApiResponse(StatusCode.Code403).Message);
        await Response.WriteAsync(message);
    }
}