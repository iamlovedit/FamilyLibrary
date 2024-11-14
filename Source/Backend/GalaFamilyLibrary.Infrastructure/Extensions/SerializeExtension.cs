using Newtonsoft.Json.Serialization;

namespace GalaFamilyLibrary.Infrastructure.Extensions;

/// <summary>
/// json序列化扩展
/// </summary>
public static class SerializeExtension
{
    static SerializeExtension()
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }

    public static string Serialize(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T? Deserialize<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}