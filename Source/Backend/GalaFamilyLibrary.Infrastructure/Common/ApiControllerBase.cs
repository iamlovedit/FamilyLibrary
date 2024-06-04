using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.Infrastructure.Common;

[ApiController]
[Authorize(Policy = PermissionConstants.POLICYNAME)]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
public class ApiControllerBase : ControllerBase
{
    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public MessageModel<T> Success<T>(T data, string message = "成功")
    {
        return new MessageModel<T>(true, message, data);
    }


    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public MessageModel<T> Failed<T>(string message = "失败", int code = 500)
    {
        return new MessageModel<T>(false, message) { StatusCode = code };
    }


    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public MessageModel<string> Failed(string message = "失败", int code = 500)
    {
        return new MessageModel<string>(false, message) { StatusCode = code };
    }


    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public MessageModel<PageModel<T>> SucceedPage<T>(int page, int dataCount, int pageSize, List<T> data, int pageCount,
        string message = "获取成功")
    {
        var pageModel = new PageModel<T>()
        {
            Data = data,
            PageCount = pageCount,
            PageSize = pageSize,
            Page = page,
            DataCount = dataCount,
        };
        return new MessageModel<PageModel<T>>(true, message, pageModel);
    }

    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public MessageModel<PageModel<T>> SucceedPage<T>(PageModel<T> page, string message = "获取成功")
    {
        var response = new PageModel<T>()
        {
            Page = page.Page,
            DataCount = page.DataCount,
            Data = page.Data,
            PageSize = page.PageSize,
            PageCount = page.PageCount,
        };
        return new MessageModel<PageModel<T>>(true, message, response);
    }
}