using System;
using HRMS.Domain.Enums;

namespace HRMS.Application.DTOs.Employees
{
    public class EmployeeUpdateDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public CivilStatus CivilStatus { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? SssNumber { get; set; }
        public string? PhilHealthNumber { get; set; }
        public string? PagIbigNumber { get; set; }
        public string? Tin { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public DateTime DateHired { get; set; }
        public Guid CompanyId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid JobTitleId { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public string? Avatar { get; set; }
    }
} 