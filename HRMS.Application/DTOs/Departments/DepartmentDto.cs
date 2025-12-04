using System;
using System.Collections.Generic;

namespace HRMS.Application.DTOs.Departments
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? HeadEmployeeId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
        public List<DepartmentHrManagerDto> HrManagers { get; set; } = new List<DepartmentHrManagerDto>();
    }
} 