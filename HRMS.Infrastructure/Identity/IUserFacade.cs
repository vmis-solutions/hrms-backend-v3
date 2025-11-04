using HRMS.Application.DTOs.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Identity
{
    public interface IUserFacade
    {
        Task<UserDto> CreateUserAsync(UserCreateDto dto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<UserDto> UpdateUserAsync(string id, UserCreateDto dto);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> AssignRoleToUserAsync(string userId, string roleName);
        Task<bool> UpdateRoleAsync(string oldRoleName, string newRoleName);
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<bool> RemoveRoleAsync(string roleName);
    }
} 