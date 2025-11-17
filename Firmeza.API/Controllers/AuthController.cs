using Firmeza.API.DTOs;
using Firmeza.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var response = await _authService.LoginAsync(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            var response = await _authService.RegisterAsync(request);
            return StatusCode(response.Code, response);
        }
    }
}
