using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserServiceApi.Services;
using UserServiceApi.Models;

namespace UserServiceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User request)
        {
            var user = await _userService.CreateUserAsync(request.Username, request.Password, request.Role);

            return Ok(new { Message = "Kullanıcı başarıyla oluşturuldu!" });
        }

        // Kullanıcı girişi
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.LoginRequest request)
        {
            var user = await _userService.ValidateUserAsync(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized(new { Message = "Geçersiz kullanıcı adı veya şifre!" });
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }


}
