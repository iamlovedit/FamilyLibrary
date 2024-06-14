namespace GalaFamilyLibrary.Infrastructure.Common;

public class MessageData<T>(bool succeed, string message, int statusCode = 200)
{
    public int StatusCode { get; set; } = statusCode;

    public bool Succeed { get; set; } = succeed;

    public string? Message { get; } = message;

    public T? Response { get; }

    public MessageData(bool succeed, string message, T response, int statusCode = 200) : this(succeed, message, statusCode)
    {
        Response = response;
    }
}