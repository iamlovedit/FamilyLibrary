namespace GalaFamilyLibrary.Infrastructure.Middlewares;

public class NotFoundMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == StatusCodes.Status404NotFound)
        {
            var message = new MessageData(false, $"路径: {context.Request.Path.Value} 不存在", 404);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(message.Serialize());
        }
    }
}