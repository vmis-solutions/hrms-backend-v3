using HRMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class Employee
    {
        // Identity reference - just a string ID, no direct dependency
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Personal Info (from Identity, duplicated for domain operations)
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public CivilStatus CivilStatus { get; set; }

        // Contact Info
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        // Government IDs
        public string? SssNumber { get; set; }
        public string? PhilHealthNumber { get; set; }
        public string? PagIbigNumber { get; set; }
        public string? Tin { get; set; }

        // Employment Info
        [Required]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        public DateTime DateHired { get; set; }

        [Required]
        public string CompanyId { get; set; } = string.Empty;

        [Required]
        public string DepartmentId { get; set; } = string.Empty;

        [Required]
        public string JobTitleId { get; set; } = string.Empty;

        [Required]
        public EmploymentStatus EmploymentStatus { get; set; }

        public string? Avatar { get; set; }

        // Navigation properties (Domain only)
        public Company Company { get; set; } = null!;
        public Department Department { get; set; } = null!;
        public JobTitle JobTitle { get; set; } = null!;
        public ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

        // Departments where this employee is head
        public ICollection<Department> DepartmentsAsHead { get; set; } = new List<Department>();

        // Leave applications where this employee is approver
        public ICollection<LeaveApplication> LeaveApplicationsAsDepartmentHead { get; set; } = new List<LeaveApplication>();
        public ICollection<LeaveApplication> LeaveApplicationsAsHrPersonnel { get; set; } = new List<LeaveApplication>();

        // Domain methods
        public string GetFullName() => $"{FirstName} {LastName}";
        public bool IsActive() => EmploymentStatus is EmploymentStatus.Regular or EmploymentStatus.Probationary;
    }
}
