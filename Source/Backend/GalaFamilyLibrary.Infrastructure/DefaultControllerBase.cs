namespace GalaFamilyLibrary.Infrastructure;

/// <summary>
/// controller基类
/// </summary>
[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
public class DefaultControllerBase : ControllerBase
{
    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public static MessageData<T> Succeed<T>(T data, string message = "成功", int code = 200)
    {
        return new MessageData<T>(true, message, data, code);
    }

    public static MessageData Succeed(string message = "成功", int code = 200)
    {
        return new MessageData(true, message, code);
    }

    public static MessageData Fail(string message = "失败", int code = 500)
    {
        return new MessageData(false, message, code);
    }

    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public static MessageData<T> Fail<T>(string message = "失败", int code = 500)
    {
        return new MessageData<T>(false, message, default(T), code);
    }


    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public static MessageData<PageData<T>> SucceedPage<T>(int page, int dataCount, int pageSize, List<T> data,
        int pageCount,
        string message = "获取成功")
    {
        var pageData = new PageData<T>()
        {
            Data = data,
            PageCount = pageCount,
            PageSize = pageSize,
            Page = page,
            DataCount = dataCount,
        };
        return new MessageData<PageData<T>>(true, message, pageData);
    }

    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public static MessageData<PageData<T>> SucceedPage<T>(PageData<T> page, string message = "获取成功")
    {
        return new MessageData<PageData<T>>(true, message, page);
    }
}