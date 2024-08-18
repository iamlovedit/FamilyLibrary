using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.Filters
{
    public class IdempotencyAttribute(int seconds = 5) : Attribute
    {
        public int Seconds { get; set; } = seconds;
    }

    public class IdempotencyFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var idempotencyAttribute =
                    controllerActionDescriptor.MethodInfo.GetCustomAttribute<IdempotencyAttribute>();
                if (idempotencyAttribute is null)
                {
                    await next();
                }

                var request = context.HttpContext.Request;
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;

                var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(body));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                var redis = request.HttpContext.RequestServices.GetService<IRedisBasketRepository>();

                var redisKey = $"{request.Path.Value}:{hashString}";

                if (await redis.Exist(redisKey))
                {
                    var message = new MessageData(false, "非法请求", 500);
                    context.Result = new ObjectResult(message) { StatusCode = 200 };
                }
                else
                {
                    await redis.Set(redisKey, 0, TimeSpan.FromSeconds(idempotencyAttribute.Seconds));
                    await next();
                }
            }
        }
    }
}