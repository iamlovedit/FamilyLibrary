using GalaFamilyLibrary.Infrastructure.Common;
using Newtonsoft.Json;

namespace GalaFamilyLibrary.FamilyService.Helpers;

public class CategoryQuery : PageQueryBase
{
    [JsonProperty(Order = 1)] public int CategoryId { get; set; }
}

public class KeywordQuery : PageQueryBase
{
    [JsonProperty(Order = 1)] public string Keyword { get; set; }
}

public class CategoryKeywordQuery : PageQueryBase
{
    [JsonProperty(Order = 1)] public string? Keyword { get; set; }
    
    [JsonProperty(Order = 2)] public int? CategoryId { get; set; }
}