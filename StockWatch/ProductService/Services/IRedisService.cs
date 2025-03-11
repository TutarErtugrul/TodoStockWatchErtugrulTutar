namespace ProductServiceAPI.Services
{
    
    public interface IRedisService
    {
        //Task PublishStockUpdateAsync(string message);
        void SubscribeToStockUpdates();
        Task NotifySignalRClients(string message);
    }

}
