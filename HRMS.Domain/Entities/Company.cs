using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class Company : BaseAuditable
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }

        // Navigation properties
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
