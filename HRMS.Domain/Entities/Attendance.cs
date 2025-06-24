using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class Attendance : BaseAuditable
    {
        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }

        [Required]
        public decimal TotalHours { get; set; }

        // Navigation properties
        public Employee Employee { get; set; } = null!;

        // Domain methods
        public bool IsFullDay() => TotalHours >= 8;
        public bool IsLate() => TimeIn?.TimeOfDay > new TimeSpan(8, 0, 0);
    }
}
