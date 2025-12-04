using System;

namespace HRMS.Application.DTOs.Departments
{
    public class DepartmentCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? HeadEmployeeId { get; set; }
    }
} 