using System.Threading.Tasks;
using HRMS.Application.DTOs.Employees;

namespace HRMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(string username, string password);
        Task<(string token, string refreshToken)> RefreshTokenAsync(RefreshTokenRequestDto dto);
        Task ForgotPasswordAsync(ForgotPasswordRequestDto dto);
        Task ResetPasswordAsync(ResetPasswordRequestDto dto);
    }
}
