using Microsoft.AspNetCore.Identity;

using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public class UserService : IUserService
    {
        private readonly IDbConnectionService _dbConnectionService;

        public UserService(IDbConnectionService dbConnectionService)
        {
            _dbConnectionService = dbConnectionService;
        }

       
        public async Task<User> CreateUserAsync(string username, string password, string role)
        {
            
            var roleIdQuery = "SELECT Id FROM Roles WHERE Name = @Role";
            var roleId = await _dbConnectionService.QueryFirstOrDefaultAsync<int>(roleIdQuery, new { Role = role });

            if (roleId == 0)
            {
                throw new Exception("Geçersiz rol");
            }

            var insertQuery = "INSERT INTO Users (Username, PasswordHash, RoleId) VALUES (@Username, @PasswordHash, @RoleId)";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            await _dbConnectionService.ExecuteAsync(insertQuery, new { Username = username, PasswordHash = passwordHash, RoleId = roleId });

            return new User { Username = username, Password = passwordHash, RoleId = roleId };
        }

        
        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var query = "SELECT u.Id, u.Username, u.PasswordHash, u.RoleId, r.Name AS Role FROM Users u " +
                        "INNER JOIN Roles r ON u.RoleId = r.Id WHERE u.Username = @Username";
            var user = await _dbConnectionService.QueryFirstOrDefaultAsync<User>(query, new { Username = username });

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }

}
