using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class DepartmentHrManager : BaseAuditable
    {
        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        // Navigation properties
        public Department Department { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }
}

