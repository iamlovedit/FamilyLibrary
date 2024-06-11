using GalaFamilyLibrary.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class RedisCacheSetup
    {
        public static void AddRedisCacheSetup(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddTransient<IRedisBasketRepository, RedisBasketRepository>();
            services.AddSingleton<RedisRequirement>(provider => new RedisRequirement(TimeSpan.FromMinutes(30)));
            services.AddSingleton<ConnectionMultiplexer>(provider =>
            {
                var redisConnectionString = $"{configuration["REDIS_HOST"]},password={configuration["REDIS_PASSWORD"]}";
                var redisConfig = ConfigurationOptions.Parse(redisConnectionString, true);
                redisConfig.ResolveDns = true;
                return ConnectionMultiplexer.Connect(redisConfig);
            });
        }
    }
}