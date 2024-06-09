using Microsoft.Extensions.DependencyInjection;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class RoutingSetup
    {
        public static void AddRoutingSetup(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
        }
    }
}
