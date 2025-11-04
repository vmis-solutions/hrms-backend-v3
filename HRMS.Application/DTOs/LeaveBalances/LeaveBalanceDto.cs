using System;
using HRMS.Domain.Enums;

namespace HRMS.Application.DTOs.LeaveBalances
{
    public class LeaveBalanceDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int Year { get; set; }
        public LeaveType LeaveType { get; set; }
        public int TotalDays { get; set; }
        public int UsedDays { get; set; }
        public int RemainingDays { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        
        // Legacy properties for backward compatibility
        public int TotalPaidLeave { get; set; }
        public int UsedPaidLeave { get; set; }
        public int RemainingPaidLeave { get; set; }
    }
} 