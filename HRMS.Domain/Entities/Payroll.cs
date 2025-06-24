using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class Payroll : BaseAuditable
    {
        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public DateTime PeriodStart { get; set; }

        [Required]
        public DateTime PeriodEnd { get; set; }

        [Required]
        public decimal BasicPay { get; set; }

        [Required]
        public decimal Allowances { get; set; }

        [Required]
        public decimal Deductions { get; set; }

        [Required]
        public decimal NetPay { get; set; }

        // Navigation properties
        public Employee Employee { get; set; } = null!;

        // Domain methods
        public decimal GrossPay => BasicPay + Allowances;
        public void CalculateNetPay() => NetPay = GrossPay - Deductions;
    }
}

