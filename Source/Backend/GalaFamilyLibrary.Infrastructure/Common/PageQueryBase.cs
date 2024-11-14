using System.Text.Json.Serialization;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class PageQueryBase
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public string OrderField { get; set; } = "name";
}