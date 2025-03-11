using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductServiceAPI.Models;

namespace ProductServiceAPI.Repositories
{

    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private readonly RabbitMQPublisher _publisher;

        public ProductRepository(IConfiguration configuration, RabbitMQPublisher publisher)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _publisher = publisher;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Product>("SELECT * FROM Products");
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Product>(
                "SELECT * FROM Products WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
            INSERT INTO Products (Name, Barcode, Category, CriticalStockLevel, CreatedAt, Location)
            VALUES (@Name, @Barcode, @Category, @CriticalStockLevel, @CreatedAt,@Location);
            SELECT CAST(SCOPE_IDENTITY() as int)";
            int idInfo = await connection.ExecuteScalarAsync<int>(sql, product);
            string sql1 = $"insert into stockmovements (ProductId, Quantity,MovementDate, OperationType) " +
                $"values ({idInfo}, 0,'{DateTime.Now}','NewProduct')";
            await connection.ExecuteAsync(sql1);
            
            return idInfo;
        }

        public async Task<int> UpdateAsync(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
            UPDATE Products 
            SET Name = @Name, Barcode = @Barcode, Category = @Category, 
                CriticalStockLevel = @CriticalStockLevel , Location=@Location 
            WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, product);
        }
        

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql1 = $"insert into stockmovements (ProductId,Quantity, MovementDate, OperationType) values ({id},0,'{DateTime.Now}','Delete')";
            await connection.ExecuteAsync(sql1);
            return await connection.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });
        }

        public async Task<string> UpdateStockAsync(int userId, UpdateStock updateStock)
        {
            using var connection = new SqlConnection(_connectionString);
            
            string sql1 = $"insert into stockmovements (ProductId, Quantity,MovementDate, OperationType,UserId) values ({updateStock.Id},{updateStock.StockQuantity},'{DateTime.Now}','{updateStock.OperationType}',{userId})";
            await connection.ExecuteAsync(sql1);
            string sql2 = $"select CriticalStockLevel from Products where Id={updateStock.Id}";
            object result = await connection.ExecuteScalarAsync(sql2);
            int CriticalStockLevel = result != DBNull.Value ? Convert.ToInt32(result) : 0;
            string sqlAdd = $"select sum(Quantity) from StockMovements where ProductId={updateStock.Id} and OperationType='Add'";
            int addQuantity = await connection.ExecuteScalarAsync<int?>(sqlAdd) ?? 0;
            string sqlRemove = $"select sum(Quantity) from StockMovements where ProductId={updateStock.Id} and OperationType='Sale'";
            int removeQuantity = await connection.ExecuteScalarAsync<int?>(sqlRemove) ?? 0;
            int currntQuantity = addQuantity - removeQuantity;
            if (CriticalStockLevel >= currntQuantity)
                _publisher.PublishStockAlert(updateStock.Id, currntQuantity);
            
            return "Ok";
        }

        public async Task<List<ReportResponse>> SumQuantitySoldAsync(DateTime start, DateTime end)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = $"select b.id ,b.name, sum(a.quantity) as result from StockMovements a join products b on b.id=a.productid where 1=1 and a.movementdate between '{start}' and '{end}' and a.operationtype='Sale' group by b.id,b.name order by 3 desc";
            var response = await connection.QueryAsync<ReportResponse>(sql);
            return response.ToList();
        }

        public async Task<List<ReportResponse>> StockTurnoverRateAsync(DateTime start, DateTime end)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = $"select b.id ,b.name, sum(a.quantity)/DATEDIFF(DAY, '{start}', '{end}') as result from StockMovements a join products b on b.id=a.productid where 1=1 and a.movementdate between '{start}' and '{end}' and a.operationtype='Sale' group by b.id,b.name order by 3 desc";
            var response = await connection.QueryAsync<ReportResponse>(sql);
            return response.ToList();
        }
    }

}
