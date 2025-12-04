using HRMS.Application.Interfaces;
using HRMS.Application.DTOs.Employees;
using HRMS.Application.DTOs.Users;
using HRMS.Application.Interfaces.Employees;
using System.Threading.Tasks;
using System.Linq;
using System;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(IJwtService jwtService, IEmployeeRepository employeeRepository, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _jwtService = jwtService;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists");

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
                throw new InvalidOperationException("Email already exists");

            // Validate password confirmation
            if (request.Password != request.ConfirmPassword)
                throw new InvalidOperationException("Password and confirmation password do not match");

            // Validate and create role if it doesn't exist
            if (string.IsNullOrWhiteSpace(request.Role))
                throw new InvalidOperationException("Role is required");

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(request.Role));
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Role creation failed: {errors}");
                }
            }

            // Create new user
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = false // Can be set to true if email verification is not required
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            // Assign role to user
            var roleAssignmentResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!roleAssignmentResult.Succeeded)
            {
                var errors = string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Role assignment failed: {errors}");
            }

            // Generate token with the assigned role
            var token = _jwtService.GenerateToken(user.Id, user.Email ?? request.Email, request.Role);

            return new RegisterResponseDto
            {
                Token = token,
                RefreshToken = "dummy-refresh-token", // TODO: Implement real refresh token
                UserId = user.Id,
                Email = user.Email ?? request.Email,
                UserName = user.UserName ?? request.UserName,
                Role = request.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60) // TODO: Get from configuration
            };
        }

        public async Task<LoginResponseDto> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid credentials");
            
            // Get user roles from Identity
            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles.FirstOrDefault() ?? "USER"; // Use first role or default to "USER"
            
            var employee = (await _employeeRepository.GetAllAsync()).FirstOrDefault(e => e.Id == user.EmployeeId);
            
            // If employee exists, use employee data; otherwise use user data
            if (employee != null)
            {
                var token = _jwtService.GenerateToken(employee.Id.ToString(), employee.Email, role);

                return new LoginResponseDto
                {
                    Token = token,
                    RefreshToken = "dummy-refresh-token", // TODO: Implement real refresh token
                    UserId = employee.Id.ToString(),
                    Email = employee.Email,
                    FullName = employee.GetFullName(),
                    Role = role,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60) // TODO: Get from configuration
                };
            }
            else
            {
                // User without employee record
                var token = _jwtService.GenerateToken(user.Id, user.Email ?? "", role);

                return new LoginResponseDto
                {
                    Token = token,
                    RefreshToken = "dummy-refresh-token", // TODO: Implement real refresh token
                    UserId = user.Id,
                    Email = user.Email ?? "",
                    FullName = user.UserName ?? "",
                    Role = role,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60) // TODO: Get from configuration
                };
            }
        }

        public Task<(string token, string refreshToken)> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            // TODO: Implement real refresh logic
            return Task.FromResult(("dummy-jwt-token", "dummy-refresh-token"));
        }

        public Task ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            // TODO: Implement real forgot password logic
            return Task.CompletedTask;
        }

        public Task ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            // TODO: Implement real reset password logic
            return Task.CompletedTask;
        }

        public async Task ChangePasswordAsync(string userId, ChangePasswordRequestDto dto)
        {
            // Validate password confirmation
            if (dto.NewPassword != dto.ConfirmNewPassword)
                throw new InvalidOperationException("New password and confirmation password do not match");

            // Find user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            // Verify current password
            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, dto.CurrentPassword, false);
            if (!passwordCheck.Succeeded)
                throw new UnauthorizedAccessException("Current password is incorrect");

            // Change password
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Password change failed: {errors}");
            }
        }

        private string DetermineUserRole(HRMS.Domain.Entities.Employee employee)
        {
            // TODO: Implement proper role determination logic
            // For now, we'll use a simple mapping based on job title
            var jobTitle = employee.JobTitle?.Title?.ToLower() ?? "";

            return jobTitle switch
            {
                var title when title.Contains("admin") => "ADMIN",
                var title when title.Contains("hr manager") => "HR_MANAGER",
                var title when title.Contains("hr supervisor") => "HR_SUPERVISOR",
                var title when title.Contains("hr") => "HR_COMPANY",
                var title when title.Contains("head") || title.Contains("manager") => "DEPARTMENT_HEAD",
                _ => "EMPLOYEE"
            };
        }
    }
}
