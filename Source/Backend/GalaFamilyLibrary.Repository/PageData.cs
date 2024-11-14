using AutoMapper;

namespace GalaFamilyLibrary.Repository;

public class PageData<T>
{
    public PageData()
    {
    }

    public PageData(int pageIndex, int pageCount, long dataCount, int pageSize, List<T> data)
    {
        Page = pageIndex;
        PageCount = pageCount;
        DataCount = dataCount;
        PageSize = pageSize;
        Data = data;
    }

    public int Page { get; set; }

    public int PageCount { get; set; }

    public long DataCount { get; set; }

    public int PageSize { get; set; }

    public List<T> Data { get; set; }

    public PageData<TResult> ConvertTo<TResult>()
    {
        var result = new PageData<TResult>(Page, PageCount, DataCount, PageSize, Data?.Adapt<List<TResult>>());
        return result;
    }
}