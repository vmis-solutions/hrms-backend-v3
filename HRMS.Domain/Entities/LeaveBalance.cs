using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class LeaveBalance : BaseAuditable
    {
        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        public int TotalPaidLeave { get; set; }

        [Required]
        public int UsedPaidLeave { get; set; }

        public int RemainingPaidLeave => TotalPaidLeave - UsedPaidLeave;

        // Navigation properties
        public Employee Employee { get; set; } = null!;

        // Domain methods
        public bool CanTakeLeave(int requestedDays) => RemainingPaidLeave >= requestedDays;

        public void DeductLeave(int days)
        {
            if (!CanTakeLeave(days))
                throw new InvalidOperationException("Insufficient leave balance");

            UsedPaidLeave += days;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
