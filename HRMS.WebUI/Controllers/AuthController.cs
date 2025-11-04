using HRMS.Application.Interfaces;
using HRMS.Application.DTOs.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController<AuthController>
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public AuthController(IConfiguration configuration, IAuthService authService, ILogger<AuthController> logger)
            : base(logger)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return CreateResponse(400, "Login data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            try
            {
                var token = await _authService.LoginAsync(request.Username, request.Password);
                
                return CreateResponse(200, "Login successful", new { token, refreshToken = "dummy-refresh-token" });
            }
            catch
            {
                return CreateResponse(401, "Invalid credentials");
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Refresh token data is required");
            var (token, refreshToken) = await _authService.RefreshTokenAsync(dto);
            return CreateResponse(200, "Token refreshed", new { token, refreshToken });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Forgot password data is required");
            await _authService.ForgotPasswordAsync(dto);
            return CreateResponse(200, "If the email exists, a reset link has been sent.");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Reset password data is required");
            await _authService.ResetPasswordAsync(dto);
            return CreateResponse(200, "Password has been reset.");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 