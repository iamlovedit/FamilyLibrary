using Newtonsoft.Json;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class MessageData<T>(bool isSuccess, string message, int statusCode = 200)
{
    public int StatusCode { get; set; } = statusCode;

    public bool IsSuccess { get; } = isSuccess;

    public string Message { get; } = message;

    public T? Response { get; }

    public MessageData(bool isSuccess, string message, T? response, int statusCode = 200) : this(isSuccess, message,
        statusCode)
    {
        Response = response;
    }
}