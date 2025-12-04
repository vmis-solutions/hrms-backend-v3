using System;

namespace HRMS.Application.DTOs.Departments
{
    public class DepartmentHrManagerDto
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeEmail { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}

