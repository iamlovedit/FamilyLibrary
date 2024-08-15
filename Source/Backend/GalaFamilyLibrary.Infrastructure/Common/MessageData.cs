namespace GalaFamilyLibrary.Infrastructure.Common;

public class MessageData(bool succeed, string message, int statusCode = 200)
{
    public string? Message { get; set; } = message;
    public int StatusCode { get; set; } = statusCode;

    public bool Succeed { get; set; } = succeed;
}

public class MessageData<T>(bool succeed, string message, T? data = default, int statusCode = 200)
    : MessageData(succeed, message, statusCode)
{
    public T? Data { get; set; } = data;
}