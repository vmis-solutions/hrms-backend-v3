using HRMS.Application.DTOs.Users;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Infrastructure.Identity
{
    public class UserFacade : IUserFacade
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserFacade(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                EmployeeId = dto.EmployeeId
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new System.Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                EmployeeId = user.EmployeeId
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return _userManager.Users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                EmployeeId = u.EmployeeId
            }).ToList();
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                EmployeeId = user.EmployeeId
            };
        }

        public async Task<UserDto> UpdateUserAsync(string id, UserCreateDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new System.Exception("User not found");
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.EmployeeId = dto.EmployeeId;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new System.Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                EmployeeId = user.EmployeeId
            };
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName)) return false;
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded;
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            if (!await _roleManager.RoleExistsAsync(roleName)) return false;
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> UpdateRoleAsync(string oldRoleName, string newRoleName)
        {
            var role = await _roleManager.FindByNameAsync(oldRoleName);
            if (role == null) return false;
            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            return _roleManager.Roles.Select(r => r.Name!).ToList();
        }

        public async Task<bool> RemoveRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return false;
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }
} 