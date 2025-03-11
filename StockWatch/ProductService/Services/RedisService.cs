using Microsoft.AspNetCore.SignalR;
using ProductServiceAPI.Hubs;
using StackExchange.Redis;

namespace ProductServiceAPI.Services
{
    // RedisService.cs
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ISubscriber _subscriber;
        private readonly IHubContext<StockHub> _hubContext;

        public RedisService(IHubContext<StockHub> hubContext)
        {
    
            _redis = ConnectionMultiplexer.Connect("localhost:6379");
            _subscriber = _redis.GetSubscriber();
            _hubContext = hubContext;
        }

 
        //public async Task PublishStockUpdateAsync(string message)
        //{
        //    await _subscriber.PublishAsync("stock_updates", message);
        //}


        public void SubscribeToStockUpdates()
        {
            _subscriber.Subscribe("stock_updates", (channel, message) =>
            {
                NotifySignalRClients(message).Wait();
            });
        }

  
        public async Task NotifySignalRClients(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveStockUpdate", message);
        }
    }

}
