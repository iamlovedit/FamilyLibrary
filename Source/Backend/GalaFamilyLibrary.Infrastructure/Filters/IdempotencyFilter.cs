using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using GalaFamilyLibrary.Infrastructure.Attributes;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Redis;
using GalaFamilyLibrary.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GalaFamilyLibrary.Infrastructure.Filters
{
    public sealed class IdempotencyFilter(ILogger<IdempotencyFilter> logger, IRedisBasketRepository redis)
        : IAsyncActionFilter
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
                    return;
                }

                if (!context.ActionArguments.TryGetValue(idempotencyAttribute!.Parameter, out var value))
                {
                    await next();
                    return;
                }

                var request = context.HttpContext.Request;
                var body = value!.Serialize();
                var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(body));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                var redisKey = $"{request.Path.Value}:{request.Method}:{hashString}";

                if (await redis.Exist(redisKey))
                {
                    logger.LogWarning("路径 {path} 请求频繁，请求ip：{ip}", request.Path,
                        context.HttpContext.GetRequestIp());
                    var message = new MessageData(false, idempotencyAttribute.Message, 409);
                    context.Result = new ObjectResult(message) { StatusCode = 200 };
                }
                else
                {
                    await redis.Set(redisKey, 0, TimeSpan.FromSeconds(idempotencyAttribute!.Seconds));
                    await next();
                }
            }
        }
    }
}