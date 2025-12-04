using HRMS.Application.Common;
using HRMS.Application.DTOs.Employees;
using HRMS.Application.DTOs.Users;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Identity
{
    public class UserFacade : IUserFacade
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserFacade(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            var validationErrors = new UserValidationErrorDto();

            // Check for duplicate username
            var existingUserByUsername = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUserByUsername != null)
            {
                validationErrors.Errors.Add(new ValidationError
                {
                    Field = nameof(dto.UserName),
                    Message = $"A user with username '{dto.UserName}' already exists.",
                    Value = dto.UserName
                });
            }

            // Check for duplicate email
            var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
            {
                validationErrors.Errors.Add(new ValidationError
                {
                    Field = nameof(dto.Email),
                    Message = $"A user with email '{dto.Email}' already exists.",
                    Value = dto.Email
                });
            }

            // If we have validation errors, throw exception before attempting to create
            if (validationErrors.Errors.Count > 0)
            {
                throw new UserValidationException(validationErrors);
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                EmployeeId = dto.EmployeeId
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                // Convert Identity errors to validation errors
                foreach (var error in result.Errors)
                {
                    // Map Identity error codes to field names
                    string field = error.Code switch
                    {
                        "DuplicateUserName" => nameof(dto.UserName),
                        "DuplicateEmail" => nameof(dto.Email),
                        "InvalidEmail" => nameof(dto.Email),
                        "PasswordTooShort" => nameof(dto.Password),
                        "PasswordRequiresNonAlphanumeric" => nameof(dto.Password),
                        "PasswordRequiresDigit" => nameof(dto.Password),
                        "PasswordRequiresLower" => nameof(dto.Password),
                        "PasswordRequiresUpper" => nameof(dto.Password),
                        _ => "General"
                    };

                    validationErrors.Errors.Add(new ValidationError
                    {
                        Field = field,
                        Message = error.Description,
                        Value = field == nameof(dto.Password) ? "***" : (field == nameof(dto.UserName) ? dto.UserName : dto.Email)
                    });
                }

                throw new UserValidationException(validationErrors);
            }

            // Ensure HR_COMPANY role exists, then assign it as default
            const string defaultRole = "HR_COMPANY";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(defaultRole));
            }

            // Assign default role HR_COMPANY
            var roleResult = await _userManager.AddToRoleAsync(user, defaultRole);
            if (!roleResult.Succeeded)
            {
                // Log warning but don't fail user creation if role assignment fails
                // The role might not exist, but user is already created
            }

            Employee? employee = null;
            if (user.EmployeeId.HasValue)
            {
                employee = await _context.Employees.FindAsync(user.EmployeeId.Value);
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                EmployeeId = user.EmployeeId,
                FirstName = employee?.FirstName,
                LastName = employee?.LastName,
                MiddleName = employee?.MiddleName,
                EmployeeNumber = employee?.EmployeeNumber
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

        public async Task<PagedResult<UserDto>> GetAllUsersPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var query = from user in _context.Users
                        join employee in _context.Employees on user.EmployeeId equals employee.Id into employeeGroup
                        from employee in employeeGroup.DefaultIfEmpty()
                        select new { User = user, Employee = employee };

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var like = $"%{searchTerm.Trim()}%";
                query = query.Where(x =>
                    EF.Functions.Like(x.User.UserName ?? string.Empty, like) ||
                    EF.Functions.Like(x.User.Email ?? string.Empty, like) ||
                    (x.Employee != null && (
                        EF.Functions.Like(x.Employee.FirstName ?? string.Empty, like) ||
                        EF.Functions.Like(x.Employee.LastName ?? string.Empty, like) ||
                        EF.Functions.Like(x.Employee.MiddleName ?? string.Empty, like) ||
                        EF.Functions.Like(x.Employee.EmployeeNumber ?? string.Empty, like) ||
                        EF.Functions.Like((x.Employee.FirstName ?? string.Empty) + " " + (x.Employee.LastName ?? string.Empty), like) ||
                        EF.Functions.Like((x.Employee.LastName ?? string.Empty) + ", " + (x.Employee.FirstName ?? string.Empty), like)
                    )));
            }

            var totalCount = await query.CountAsync();

            var results = await query
                .OrderBy(x => x.User.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<UserDto>
            {
                Items = results.Select(x => new UserDto
                {
                    Id = x.User.Id,
                    UserName = x.User.UserName ?? string.Empty,
                    Email = x.User.Email ?? string.Empty,
                    EmployeeId = x.User.EmployeeId,
                    FirstName = x.Employee != null ? x.Employee.FirstName : null,
                    LastName = x.Employee != null ? x.Employee.LastName : null,
                    MiddleName = x.Employee != null ? x.Employee.MiddleName : null,
                    EmployeeNumber = x.Employee != null ? x.Employee.EmployeeNumber : null
                }),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            Employee? employee = null;
            if (user.EmployeeId.HasValue)
            {
                employee = await _context.Employees.FindAsync(user.EmployeeId.Value);
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                EmployeeId = user.EmployeeId,
                FirstName = employee?.FirstName,
                LastName = employee?.LastName,
                MiddleName = employee?.MiddleName,
                EmployeeNumber = employee?.EmployeeNumber
            };
        }

        public async Task<UserDto> UpdateUserAsync(string id, UserCreateDto dto)
        {
            var validationErrors = new UserValidationErrorDto();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new System.Exception("User not found");

            // Check for duplicate username (if changed)
            if (user.UserName != dto.UserName)
            {
                var existingUserByUsername = await _userManager.FindByNameAsync(dto.UserName);
                if (existingUserByUsername != null)
                {
                    validationErrors.Errors.Add(new ValidationError
                    {
                        Field = nameof(dto.UserName),
                        Message = $"A user with username '{dto.UserName}' already exists.",
                        Value = dto.UserName
                    });
                }
            }

            // Check for duplicate email (if changed)
            if (user.Email != dto.Email)
            {
                var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUserByEmail != null)
                {
                    validationErrors.Errors.Add(new ValidationError
                    {
                        Field = nameof(dto.Email),
                        Message = $"A user with email '{dto.Email}' already exists.",
                        Value = dto.Email
                    });
                }
            }

            // If we have validation errors, throw exception before attempting to update
            if (validationErrors.Errors.Count > 0)
            {
                throw new UserValidationException(validationErrors);
            }

            // Update user properties
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.EmployeeId = dto.EmployeeId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Convert Identity errors to validation errors
                foreach (var error in result.Errors)
                {
                    string field = error.Code switch
                    {
                        "DuplicateUserName" => nameof(dto.UserName),
                        "DuplicateEmail" => nameof(dto.Email),
                        "InvalidEmail" => nameof(dto.Email),
                        _ => "General"
                    };

                    validationErrors.Errors.Add(new ValidationError
                    {
                        Field = field,
                        Message = error.Description,
                        Value = field == nameof(dto.UserName) ? dto.UserName : dto.Email
                    });
                }

                throw new UserValidationException(validationErrors);
            }

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                // Remove existing password and add new one (admin password reset)
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (removePasswordResult.Succeeded)
                {
                    var addPasswordResult = await _userManager.AddPasswordAsync(user, dto.Password);
                    if (!addPasswordResult.Succeeded)
                    {
                        // Convert password errors to validation errors
                        foreach (var error in addPasswordResult.Errors)
                        {
                            validationErrors.Errors.Add(new ValidationError
                            {
                                Field = nameof(dto.Password),
                                Message = error.Description,
                                Value = "***"
                            });
                        }

                        throw new UserValidationException(validationErrors);
                    }
                }
                else
                {
                    // If user doesn't have a password, try to add it directly
                    var addPasswordResult = await _userManager.AddPasswordAsync(user, dto.Password);
                    if (!addPasswordResult.Succeeded)
                    {
                        foreach (var error in addPasswordResult.Errors)
                        {
                            validationErrors.Errors.Add(new ValidationError
                            {
                                Field = nameof(dto.Password),
                                Message = error.Description,
                                Value = "***"
                            });
                        }

                        throw new UserValidationException(validationErrors);
                    }
                }
            }

            Employee? employee = null;
            if (user.EmployeeId.HasValue)
            {
                employee = await _context.Employees.FindAsync(user.EmployeeId.Value);
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                EmployeeId = user.EmployeeId,
                FirstName = employee?.FirstName,
                LastName = employee?.LastName,
                MiddleName = employee?.MiddleName,
                EmployeeNumber = employee?.EmployeeNumber
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