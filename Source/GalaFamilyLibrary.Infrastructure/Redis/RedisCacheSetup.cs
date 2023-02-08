using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace GalaFamilyLibrary.Infrastructure.Redis
{
    public static class RedisCacheSetup
    {
        public static void AddRedisCacheSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IRedisBasketRepository, RedisBasketRepository>();

            services.AddSingleton<ConnectionMultiplexer>(builder =>
            {
                var redisConnectionString = configuration.GetSection("Redis")["ConnectionString"];
                var redisConfig = ConfigurationOptions.Parse(redisConnectionString, true);
                redisConfig.ResolveDns = true;
                return ConnectionMultiplexer.Connect(redisConfig);
            });
        }
    }
}
