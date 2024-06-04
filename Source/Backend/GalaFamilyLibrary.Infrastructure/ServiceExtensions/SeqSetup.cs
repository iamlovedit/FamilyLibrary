using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions;

public static class SeqSetup
{
    public static void AddSeqSetup(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        services.AddLogging(loggerBuilder =>
        {
            var seqConfig = configuration.GetSection("Seq");
            loggerBuilder.AddSeq(seqConfig);

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .WriteTo.Console()
               .WriteTo.File(Path.Combine("logs", "log"), rollingInterval: RollingInterval.Hour)
               .CreateLogger();

            loggerBuilder.AddSerilog(Log.Logger);
        });
    }
}