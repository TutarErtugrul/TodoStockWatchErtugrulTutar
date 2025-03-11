using StackExchange.Redis;

namespace ProductServiceAPI.Services
{
    public class RedisPublisherService : IRedisPublisherService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly string _channel = "stock_updates";

        public RedisPublisherService(IConfiguration configuration)
        {
            var redisHost = configuration["Redis:Host"];
            var redisPort = configuration["Redis:Port"];
            _redis = ConnectionMultiplexer.Connect($"{redisHost}:{redisPort}");
        }

        public async Task PublishStockUpdateAsync(string message)
        {
            var subscriber = _redis.GetSubscriber();
            await subscriber.PublishAsync(_channel, message);
        }
    }
}
