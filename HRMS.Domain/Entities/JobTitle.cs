using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class JobTitle : BaseAuditable
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        // Navigation properties
        public Department Department { get; set; } = null!;
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
