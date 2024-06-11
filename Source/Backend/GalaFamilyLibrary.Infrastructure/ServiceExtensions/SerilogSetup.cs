using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class SerilogSetup
    {
        public static void AddSerilogSetup(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", "log"), rollingInterval: RollingInterval.Hour)
                .WriteTo.Seq(builder.Configuration["SEQ_URL"]!, apiKey: builder.Configuration["SEQ_APIKEY"])
                .CreateLogger();
            
            builder.Services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                logBuilder.AddSerilog(Log.Logger);
            });

            builder.Host.UseSerilog(Log.Logger, true);
        }
    }
}