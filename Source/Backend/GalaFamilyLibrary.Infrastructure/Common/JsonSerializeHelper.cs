using System.Diagnostics;
using Newtonsoft.Json.Serialization;

namespace GalaFamilyLibrary.Infrastructure.Common
{
    public static class JsonSerializeHelper
    {
        static JsonSerializeHelper()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
        }

        public static string Serialize(this object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return string.Empty;
            }
        }

        public static T? DeSerialize<T>(this string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                return default;
            }
        }
    }
}