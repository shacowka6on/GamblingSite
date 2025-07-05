using GamblingSite.Core.DTOs;
using GamblingSite.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamblingSite.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try
            {
                var result = await _authService.Register(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await _authService.Login(dto.Email, dto.Password);
                return Ok(new {Token = token});
            }
            catch(ArgumentException ex) 
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpGet("test")]
        public async Task<IActionResult> TestAuth()
        {
            return Ok("You're authenticated");
        }
    }
}
