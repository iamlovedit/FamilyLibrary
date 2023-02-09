using Ocelot.Infrastructure.Extensions;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responder;

namespace GalaFamilyLibrary.Gateway.Middlewares;

public class ResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IErrorsToHttpStatusCodeMapper _statusCodeMapper;
    private readonly IHttpResponder _httpResponder;

    public ResponseMiddleware(RequestDelegate next,
        IErrorsToHttpStatusCodeMapper statusCodeMapper, IHttpResponder httpResponder)
    {
        _next = next;
        _statusCodeMapper = statusCodeMapper;
        _httpResponder = httpResponder;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        await _next.Invoke(httpContext);
        if (httpContext.Response.HasStarted)
        {
            return;
        }

        var errors = httpContext.Items.Errors();
        if (errors.Any())
        {
            // Logger.LogWarning(errors.ToErrorString());
            var statusCode = _statusCodeMapper.Map(errors);
            var error = string.Join(',', errors.Select(e => e.Message));
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(error);
        }
        else
        {
            var downstreamResponses = httpContext.Items.DownstreamResponse();
            await _httpResponder.SetResponseOnHttpContext(httpContext, downstreamResponses);
        }
    }
}