using Microsoft.AspNetCore.Http;

namespace GalaFamilyLibrary.Infrastructure.Utils
{
    public static class HttpContextExtensions
    {
        public static string GetRequestIp(this HttpContext context)
        {
            var ip = context.GetRequestHeaderValue<string>("X-Forwarded-For");
            if (!string.IsNullOrEmpty(ip) || context.Connection.RemoteIpAddress == null)
            {
                return ip;
            }

            ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.GetRequestHeaderValue<string>("REMOTE_ADDR");
            }

            return ip;
        }

        public static T? GetRequestHeaderValue<T>(this HttpContext context, string headerName)
        {
            if (!context.Request.Headers.TryGetValue(headerName, out var value))
            {
                return default;
            }

            var valueStr = value.ToString();
            if (!string.IsNullOrEmpty(valueStr) || !string.IsNullOrWhiteSpace(valueStr))
            {
                return (T)Convert.ChangeType(valueStr, typeof(T));
            }

            return default;
        }
    }
}