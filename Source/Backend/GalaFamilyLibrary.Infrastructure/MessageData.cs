namespace GalaFamilyLibrary.Infrastructure;

/// <summary>
/// 客户端返回统一封装
/// </summary>
/// <param name="successful"></param>
/// <param name="message"></param>
/// <param name="code"></param>
public class MessageData(bool succeed, string message, int code = 200)
{
    public int Code { get; set; } = code;

    public bool Succeed { get; set; } = succeed;

    public string? Message { get; set; } = message;
}

public class MessageData<T>(bool succeed, string message, T response, int code = 200)
    : MessageData(succeed, message,
        code)
{
    public T? Response { get; set; } = response;
}