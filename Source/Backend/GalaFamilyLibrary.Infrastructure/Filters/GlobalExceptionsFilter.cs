using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;

namespace GalaFamilyLibrary.Infrastructure.Filters
{
    public class GlobalExceptionsFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            return Task.Run(() =>
            {
                if (!context.ExceptionHandled)
                {
                    Log.Logger.Error($"{context.Exception.StackTrace}");
                    var message = new MessageModel<Exception>(false, context.Exception.Message, 500);
                    context.Result = new ContentResult
                    {
                        StatusCode = StatusCodes.Status200OK,
                        ContentType = "application/json;charset=utf-8",
                        Content = JsonConvert.SerializeObject(message)
                    };
                }

                //不再继续传递异常
                context.ExceptionHandled = true;
            });
        }
    }
}