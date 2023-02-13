using Newtonsoft.Json;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class MessageModel<T>
{
    [JsonProperty("code")] public int StatusCode { get; set; } = 200;
    
    [JsonProperty("success")] public bool IsSuccess { get; }
    
    [JsonProperty("message")] public string Message { get; }
    
    [JsonProperty("response")] public T Response { get; }
    
    public MessageModel(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public MessageModel(bool isSuccess, string message, T response) : this(isSuccess, message)
    {
        Response = response;
    }
}