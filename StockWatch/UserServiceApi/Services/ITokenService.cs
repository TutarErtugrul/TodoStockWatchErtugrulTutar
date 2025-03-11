using System.Security.Claims;
using UserServiceApi.Models;

namespace UserServiceApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user); 
        ClaimsPrincipal ValidateToken(string token); 
    }

}
