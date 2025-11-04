using HRMS.Domain.Enums;
using System.Security.Claims;

namespace HRMS.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email, string role);
        ClaimsPrincipal? ValidateToken(string token);
        string? GetUserIdFromToken(string token);
        //string? GetUserRoleFromToken(string token);
    }
} 