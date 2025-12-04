using HRMS.Application.Interfaces;
using HRMS.Application.DTOs.Employees;
using HRMS.Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (request == null)
                return CreateResponse(400, "Registration data is required");
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            try
            {
                var response = await _authService.RegisterAsync(request);
                return CreateResponse(201, "Registration successful", response);
            }
            catch (InvalidOperationException ex)
            {
                return CreateResponse(400, ex.Message);
            }
            catch (Exception ex)
            {
                return CreateResponse(500, "An error occurred during registration", ex.Message);
            }
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
                var response = await _authService.LoginAsync(request.Username, request.Password);
                
                return CreateResponse(200, "Login successful", response);
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

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
        {
            if (dto == null)
                return CreateResponse(400, "Change password data is required");
            
            if (!ModelState.IsValid)
                return CreateResponse(400, "Validation failed", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            
            try
            {
                // Get user ID from JWT token claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
                if (string.IsNullOrEmpty(userId))
                    return CreateResponse(401, "User identifier not found in token");

                await _authService.ChangePasswordAsync(userId, dto);
                return CreateResponse(200, "Password changed successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                return CreateResponse(401, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return CreateResponse(400, ex.Message);
            }
            catch (Exception ex)
            {
                return CreateResponse(500, "An error occurred while changing password", ex.Message);
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 