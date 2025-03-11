using Newtonsoft.Json;
using ProductServiceAPI.DTOs;
using ProductServiceAPI.Models;
using ProductServiceAPI.Repositories;
using static Dapper.SqlMapper;

namespace ProductServiceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisPublisherService _redisPublisherService;
        private readonly IRedisService _redisService;
        public ProductService(IProductRepository productRepository, IRedisPublisherService redisPublisherService, IRedisService redisService)
        {
            _productRepository = productRepository;
            _redisPublisherService = redisPublisherService;
            _redisService = redisService;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var message = JsonConvert.SerializeObject(new
            {
                ProductId = "DenemeTest",
                products
            });

            //await _redisPublisherService.PublishStockUpdateAsync(message);
            //_redisService.SubscribeToStockUpdates();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Barcode = p.Barcode,
                Category = p.Category,
                CriticalStockLevel = p.CriticalStockLevel,
                Location = p.Location
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Barcode = product.Barcode,
                Category = product.Category,
                CriticalStockLevel = product.CriticalStockLevel
            };
        }

        public async Task<int> AddProductAsync(CreateProductDto product)
        {
            var entity = new Product
            {
                Name = product.Name,
                Barcode = product.Barcode,
                Category = product.Category,
                Location = product.Location,
                CriticalStockLevel = product.CriticalStockLevel
            };
            var message = JsonConvert.SerializeObject(new
            {
                entity,
                Operation = "Create"
            });

            await _redisPublisherService.PublishStockUpdateAsync(message);
            _redisService.SubscribeToStockUpdates();
            return await _productRepository.AddAsync(entity);
        }

        public async Task<int> UpdateProductAsync(int id, UpdateProductDto product)
        {
            var entity = new Product
            {
                Id = id,
                Name = product.Name,
                Barcode = product.Barcode,
                Category = product.Category,
                CriticalStockLevel = product.CriticalStockLevel,
                Location = product.Location
            };
            var message = JsonConvert.SerializeObject(new
            {
                entity,
                Operation = "Update"
            });

            await _redisPublisherService.PublishStockUpdateAsync(message);
            _redisService.SubscribeToStockUpdates();
            return await _productRepository.UpdateAsync(entity);
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            var message = JsonConvert.SerializeObject(new
            {
                Id = id,
                Operation = "Delete"
            });

            await _redisPublisherService.PublishStockUpdateAsync(message);
            _redisService.SubscribeToStockUpdates();
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<string> UpdateStockAsync(int userId,UpdateStock updateStock)
        {
            var message = JsonConvert.SerializeObject(new
            {
                Id = updateStock.Id,
                Operation = updateStock.OperationType,
                StockChange = updateStock.StockQuantity,
                userId = userId
            });

            await _redisPublisherService.PublishStockUpdateAsync(message);
            _redisService.SubscribeToStockUpdates();
            return await _productRepository.UpdateStockAsync(userId,updateStock);
        }

        public async Task<List<ReportResponse>> SumQuantitySoldAsync(DateTime start, DateTime end)
        {
            return await _productRepository.SumQuantitySoldAsync(start, end);
        }

        public async Task<List<ReportResponse>> StockTurnoverRateAsync(DateTime start, DateTime end)
        {
            return await _productRepository.StockTurnoverRateAsync(start, end);
        }
    }
}
