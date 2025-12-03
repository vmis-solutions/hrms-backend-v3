using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Application.DTOs.Departments
{
    public class AssignHrManagersDto
    {
        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public List<Guid> EmployeeIds { get; set; } = new List<Guid>();
    }
}

