using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;

namespace GalaFamilyLibrary.Infrastructure.Redis
{
    public class RedisBasketRepository : IRedisBasketRepository
    {
        private readonly ILogger<RedisBasketRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisBasketRepository(ILogger<RedisBasketRepository> logger, ConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
            _database = redis.GetDatabase();
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public async Task Clear()
        {
            foreach (var endPoint in _redis.GetEndPoints())
            {
                var server = GetServer();
                foreach (var key in server.Keys())
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
        }

        public async Task<bool> Exist(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task<string> GetValue(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task Remove(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                if (value is string cacheValue)
                {
                    await _database.StringSetAsync(key, cacheValue, cacheTime);
                }
                else
                {
                    var jsonString = JsonConvert.SerializeObject(value);
                    var buffer = Encoding.UTF8.GetBytes(jsonString);
                    await _database.StringSetAsync(key, buffer, cacheTime);
                }
            }
        }

        public async Task<TEntity> Get<TEntity>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.HasValue)
            {
                var jsonString = Encoding.UTF8.GetString(value);
                return JsonConvert.DeserializeObject<TEntity>(jsonString);
            }
            else
            {
                return default(TEntity);
            }
        }

        public async Task<RedisValue[]> ListRangeAsync(string redisKey)
        {
            return await _database.ListRangeAsync(redisKey);
        }

        public async Task<long> ListLeftPushAsync(string redisKey, string redisValue, int db = -1)
        {
            return await _database.ListLeftPushAsync(redisKey, redisValue);
        }

        public async Task<long> ListRightPushAsync(string redisKey, string redisValue, int db = -1)
        {
            return await _database.ListRightPushAsync(redisKey, redisValue);
        }

        public async Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue, int db = -1)
        {
            var redislist = new List<RedisValue>();
            foreach (var item in redisValue)
            {
                redislist.Add(item);
            }
            return await _database.ListRightPushAsync(redisKey, redislist.ToArray());
        }

        public async Task<T> ListLeftPopAsync<T>(string redisKey, int db = -1) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await _database.ListLeftPopAsync(redisKey));
        }

        public async Task<T> ListRightPopAsync<T>(string redisKey, int db = -1) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await _database.ListRightPopAsync(redisKey));
        }

        public async Task<string> ListLeftPopAsync(string redisKey, int db = -1)
        {
            return await _database.ListLeftPopAsync(redisKey);
        }

        public async Task<string> ListRightPopAsync(string redisKey, int db = -1)
        {
            return await _database.ListRightPopAsync(redisKey);
        }

        public async Task<long> ListLengthAsync(string redisKey, int db = -1)
        {
            return await _database.ListLengthAsync(redisKey);
        }

        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1)
        {
            var result = await _database.ListRangeAsync(redisKey);
            return result.Select(o => o.ToString());
        }

        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop, int db = -1)
        {
            var result = await _database.ListRangeAsync(redisKey, start, stop);
            return result.Select(o => o.ToString());
        }

        public async Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0, int db = -1)
        {
            return await _database.ListRemoveAsync(redisKey, redisValue, type);
        }

        public async Task ListClearAsync(string redisKey, int db = -1)
        {
            await _database.ListTrimAsync(redisKey, 1, 0);
        }
    }
}
