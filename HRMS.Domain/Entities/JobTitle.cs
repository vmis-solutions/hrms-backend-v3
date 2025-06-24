using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class JobTitle
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string DepartmentId { get; set; } = string.Empty;

        // Navigation properties
        public Department Department { get; set; } = null!;
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
