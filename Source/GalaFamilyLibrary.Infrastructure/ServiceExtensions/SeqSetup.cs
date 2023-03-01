using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
        });
    }
}