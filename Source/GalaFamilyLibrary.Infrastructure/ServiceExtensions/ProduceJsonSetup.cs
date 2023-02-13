using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class ProduceJsonSetup
{
    public static void AddProduceJsonSetup(this IMvcBuilder mvcBuilder)
    {
        if (mvcBuilder == null) throw new ArgumentNullException(nameof(mvcBuilder));
        mvcBuilder.AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        });
    }
}