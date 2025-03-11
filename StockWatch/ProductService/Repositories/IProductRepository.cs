using ProductServiceAPI.Models;

namespace ProductServiceAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<int> AddAsync(Product product);
        Task<int> UpdateAsync(Product product);
        Task<string> UpdateStockAsync(int userId, UpdateStock updateStock);
        Task<int> DeleteAsync(int id);
        Task<List<ReportResponse>> SumQuantitySoldAsync(DateTime start, DateTime end);
        Task<List<ReportResponse>> StockTurnoverRateAsync(DateTime start, DateTime end);
    }

}
