using GalaFamilyLibrary.Infrastructure.Exceptions;

namespace GalaFamilyLibrary.Infrastructure.Filters
{
    public class ExceptionsFilter(ILogger<ExceptionsFilter> logger) : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return Task.CompletedTask;
            }

            var message = default(MessageData);
            if (context.Exception is FriendlyException fe)
            {
                message = fe.ConvertToMessage();
            }
            else
            {
                logger.LogError(context.Exception,context.Exception.Message);
                message = new MessageData(false, "unexpected error");
            }

            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json;charset=utf-8",
                Content = message.Serialize()
            };
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}