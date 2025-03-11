namespace ProductServiceAPI.Services
{
    public interface IRedisPublisherService
    {
        Task PublishStockUpdateAsync(string message);
    }
}
