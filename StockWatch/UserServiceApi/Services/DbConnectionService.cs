using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace UserServiceApi.Services
{
    public class DbConnectionService : IDbConnectionService
    {
        private readonly string _connectionString;

        public DbConnectionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

       
        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        
        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(sql, param);
            }
        }

       
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
            }
        }

        
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>(sql, param);
            }
        }
    }
}
