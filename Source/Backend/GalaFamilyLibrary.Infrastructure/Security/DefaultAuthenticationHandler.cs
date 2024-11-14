using System.Text.Encodings.Web;

namespace GalaFamilyLibrary.Infrastructure.Security;

/// <summary>
/// 用于统一处理验证和授权
/// </summary>
/// <param name="options"></param>
/// <param name="logger"></param>
/// <param name="encoder"></param>
public class DefaultAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string Message = "You are not authorized to access this resource";
    private const string ContentType = "application/json";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.ContentType = ContentType;
        Response.StatusCode = StatusCodes.Status200OK;
        await Response.WriteAsync(new MessageData(false, Message, 403)
            .Serialize());
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.ContentType = ContentType;
        Response.StatusCode = StatusCodes.Status200OK;
        await Response.WriteAsync(new MessageData(false, Message, 401)
            .Serialize());
    }
}