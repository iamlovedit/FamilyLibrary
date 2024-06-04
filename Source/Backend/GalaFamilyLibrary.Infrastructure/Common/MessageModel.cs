using Newtonsoft.Json;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class MessageModel<T>
{
    [JsonProperty("code")] public int StatusCode { get; set; } = 200;

    [JsonProperty("success")] public bool IsSuccess { get; }

    [JsonProperty("message")] public string Message { get; }

    [JsonProperty("response")] public T Response { get; }

    public MessageModel(bool isSuccess, string message, int statusCode = 200)
    {
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
    }

    public MessageModel(bool isSuccess, string message, T response, int statusCode = 200) : this(isSuccess, message, statusCode)
    {
        Response = response;
    }
}