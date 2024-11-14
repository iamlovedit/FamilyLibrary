namespace GalaFamilyLibrary.Infrastructure.Extensions;

/// <summary>
/// HttpContext扩展
/// </summary>
public static class HttpContextExtension
{
    /// <summary>
    /// 用于获取用户访问ip
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? GetRequestIp(this HttpContext context)
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

    /// <summary>
    /// 获取header值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="headerName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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