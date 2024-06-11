using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.Infrastructure.Common;

[ApiController]
[Authorize(Policy = PermissionConstants.POLICYNAME)]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
public class GalaControllerBase : ControllerBase
{
    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    protected static MessageData<T?> Success<T>(T data, string message = "成功")
    {
        return new MessageData<T?>(true, message, data);
    }


    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    protected static MessageData<T> Failed<T>(string message = "失败", int code = 500)
    {
        return new MessageData<T>(false, message) { StatusCode = code };
    }


    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    protected static MessageData<string?> Failed(string message = "失败", int code = 500)
    {
        return new MessageData<string?>(false, message) { StatusCode = code };
    }


    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    protected static MessageData<PageData<T>?> SucceedPage<T>(int page, int dataCount, int pageSize, List<T> data,
        int pageCount,
        string message = "获取成功")
    {
        var pageModel = new PageData<T>()
        {
            Data = data,
            PageCount = pageCount,
            PageSize = pageSize,
            Page = page,
            DataCount = dataCount,
        };
        return new MessageData<PageData<T>?>(true, message, pageModel);
    }

    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    protected static MessageData<PageData<T>?> SucceedPage<T>(PageData<T>? page, string message = "获取成功")
    {
        var response = new PageData<T>()
        {
            Page = page.Page,
            DataCount = page.DataCount,
            Data = page.Data,
            PageSize = page.PageSize,
            PageCount = page.PageCount,
        };
        return new MessageData<PageData<T>?>(true, message, response);
    }
}