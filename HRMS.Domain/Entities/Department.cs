using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class Department : BaseAuditable
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        public Guid? HeadEmployeeId { get; set; }

        // Navigation properties
        public Company Company { get; set; } = null!;
        public Employee? Head { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<JobTitle> JobTitles { get; set; } = new List<JobTitle>();
        public ICollection<DepartmentHrManager> HrManagers { get; set; } = new List<DepartmentHrManager>();
    }
}
