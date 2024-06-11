using AutoMapper;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class PageData<T>
{
    public PageData()
    {

    }
    public PageData(int pageIndex, int pageCount, int dataCount, int pageSize, List<T> data)
    {
        Page = pageIndex;
        PageCount = pageCount;
        DataCount = dataCount;
        PageSize = pageSize;
        Data = data;
    }

    public int Page { get; set; }

    public int PageCount { get; set; }
  
    public int DataCount { get; set; }

    public int PageSize { get; set; }

    public List<T>? Data { get; set; }


    private PageData<TResult> ConvertTo<TResult>()
    {
        return new PageData<TResult>(Page, PageCount, DataCount, PageSize, default);
    }

    public PageData<TResult> ConvertTo<TResult>(IMapper mapper)
    {
        var result = ConvertTo<TResult>();
        if (Data != null)
        {
            result.Data = mapper.Map<List<TResult>>(Data);
        }
        return result;
    }
}