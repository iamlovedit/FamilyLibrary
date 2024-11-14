namespace GalaFamilyLibrary.Infrastructure.Exceptions;

/// <summary>
/// 返回客户端的友好错误提示
/// </summary>
/// <param name="message">错误信息</param>
/// <param name="code">错误code</param>
public sealed class FriendlyException(string message, int code = 500) : Exception
{
    public string? Message { get; set; } = message;

    public int Code { get; set; } = code;

    public MessageData ConvertToMessage()
    {
        return new MessageData(false, Message, Code);
    }
}