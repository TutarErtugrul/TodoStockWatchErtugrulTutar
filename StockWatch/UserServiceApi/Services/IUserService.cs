using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string username, string password, string role);
        Task<User> ValidateUserAsync(string username, string password);
    }

}
