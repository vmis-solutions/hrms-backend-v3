using System;

namespace HRMS.Application.DTOs.Dashboard
{
    public class EmployeeChart
    {
        public int TotalEmployee { get; set; }
        public int RegularEmployee { get; set; }
        public int ProbitionalEmployee { get; set; }
        public int ContractualEmployee { get; set; }
        
        // Additional properties expected by services
        public int TotalEmployees { get; set; }
        public int NewHires { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
} 