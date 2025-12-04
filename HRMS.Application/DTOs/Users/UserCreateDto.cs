using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Application.DTOs.Users
{
    public class UserCreateDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public Guid? EmployeeId { get; set; }
    }
} 