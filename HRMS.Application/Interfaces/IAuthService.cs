using System.Threading.Tasks;
using HRMS.Application.DTOs.Employees;
using HRMS.Application.DTOs.Users;

namespace HRMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginAsync(string username, string password);
        Task<(string token, string refreshToken)> RefreshTokenAsync(RefreshTokenRequestDto dto);
        Task ForgotPasswordAsync(ForgotPasswordRequestDto dto);
        Task ResetPasswordAsync(ResetPasswordRequestDto dto);
        Task ChangePasswordAsync(string userId, ChangePasswordRequestDto dto);
    }
}
