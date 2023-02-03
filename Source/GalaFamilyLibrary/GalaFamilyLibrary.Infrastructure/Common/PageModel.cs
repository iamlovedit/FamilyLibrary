using AutoMapper;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class PageModel<T>
{
    public PageModel()
    {

    }
    public PageModel(int pageIndex, int pageCount, int dataCount, int pageSize, List<T> data)
    {
        Page = pageIndex;
        PageCount = pageCount;
        DataCount = dataCount;
        PageSize = pageSize;
        Data = data;
    }
    /// <summary>
    /// 当前页
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// 总页数
    /// </summary>
    public int PageCount { get; set; }
    /// <summary>
    /// 数据总量
    /// </summary>
    public int DataCount { get; set; }
    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// 返回数据
    /// </summary>
    public List<T> Data { get; set; }


    public PageModel<TResult> ConvertTo<TResult>()
    {
        return new PageModel<TResult>(Page, PageCount, DataCount, PageSize, default);
    }

    public PageModel<TResult> ConvertTo<TResult>(IMapper mapper)
    {
        var result = ConvertTo<TResult>();
        if (Data != null)
        {
            result.Data = mapper.Map<List<TResult>>(Data);
        }
        return result;
    }
}