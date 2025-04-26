using BAL.DTOs.Authentication;
using BAL.Services.Interface;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserService _userService;

        public AuthenticationController(IAuthenticationService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Username and password are required.");
                }

                var user = await _authService.LoginAsync(request.Email, request.Password);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login-jwt")]
        public async Task<IActionResult> LoginJwt([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Username and password are required.");
                }

                LoginDto loginDto = new LoginDto(request.Email, request.Password);

                var user = await _userService.LoginAsync(loginDto);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Username and password are required.");
                }

                var user = await _userService.RegisterAsync(request);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // Log tất cả claims trong token
                Console.WriteLine("Claims in token:");
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }

                var user = await _userService.GetProfileAsync();
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}