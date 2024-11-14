using System.Text.Encodings.Web;

namespace GalaFamilyLibrary.Infrastructure.Security;

public class GalaAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = StatusCodes.Status200OK;
        var message = new GalaApiResponse(StatusCode.Code401).Message.Serialize();
        await Response.WriteAsync(message);
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = StatusCodes.Status200OK;
        var message = new GalaApiResponse(StatusCode.Code403).Message.Serialize();
        await Response.WriteAsync(message);
    }
}