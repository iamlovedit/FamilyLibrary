using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class TraceSetup
    {
        public static void AddTraceOutputSetup(this WebApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var configuration = app.Configuration;
            Console.WriteLine($"Current Environment: {app.Environment.EnvironmentName}");
            Console.WriteLine($"Database connection:{configuration["DATABASE_CONNECTION_STRING"]}");
            Console.WriteLine($"Redis connection:{configuration["REDIS_CONNECTION_STRING"]}");
        }
    }
}
