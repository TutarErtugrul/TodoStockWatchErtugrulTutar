using ProductServiceAPI.DTOs;
using ProductServiceAPI.Models;

namespace ProductServiceAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<int> AddProductAsync(CreateProductDto product);
        Task<int> UpdateProductAsync(int id, UpdateProductDto product);
        Task<string> UpdateStockAsync(int userId,UpdateStock updateStock);
        Task<int> DeleteProductAsync(int id);
        Task<List<ReportResponse>> SumQuantitySoldAsync(DateTime start, DateTime end);
        Task<List<ReportResponse>> StockTurnoverRateAsync(DateTime start, DateTime end);
    }


}
