using HRMS.Application.Interfaces;
using HRMS.Application.DTOs.Employees;
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

        public AuthService(IJwtService jwtService, IEmployeeRepository employeeRepository, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _jwtService = jwtService;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginResponseDto> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid credentials");
            
            var employee = (await _employeeRepository.GetAllAsync()).FirstOrDefault(e => e.Id == user.EmployeeId);
            if (employee == null)
                throw new UnauthorizedAccessException("Employee not found");
            
            var role = DetermineUserRole(employee);

            var token = _jwtService.GenerateToken(employee.Id.ToString(), employee.Email, role);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = "dummy-refresh-token", // TODO: Implement real refresh token
                UserId = employee.Id.ToString(),
                Email = employee.Email,
                FullName = employee.GetFullName(),
                //Role = role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60) // TODO: Get from configuration
            };
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
