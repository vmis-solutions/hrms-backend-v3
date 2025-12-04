using System;

namespace HRMS.Application.DTOs.Departments
{
    public class DepartmentUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? HeadEmployeeId { get; set; }
    }
} 