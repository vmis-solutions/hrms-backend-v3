using System;
using System.Collections.Generic;
using HRMS.Domain.Enums;

namespace HRMS.Application.DTOs.LeaveApplications
{
    public class LeaveApplicationCreateDto
    {
        public Guid EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public int PaidDays { get; set; }
        public int UnpaidDays { get; set; }
        public List<LeaveDayDto> LeaveDays { get; set; } = new List<LeaveDayDto>();
        public string Reason { get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; }
        public Guid? DepartmentHeadId { get; set; }
        public Guid? HrPersonnelId { get; set; }
    }
} 