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
            services.AddSingleton<RedisRequirement>(provider => new RedisRequirement(TimeSpan.FromMinutes(30)));
            services.AddSingleton<ConnectionMultiplexer>(provider =>
            {
                var redisConnectionString = configuration["REDIS_CONNECTION_STRING"];
                var redisConfig = ConfigurationOptions.Parse(redisConnectionString, true);
                redisConfig.ResolveDns = true;
                return ConnectionMultiplexer.Connect(redisConfig);
            });
        }
    }
}