using System;
using System.Collections.Generic;
using HRMS.Domain.Enums;

namespace HRMS.Application.DTOs.LeaveApplications
{
    public class LeaveApplicationDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public int PaidDays { get; set; }
        public int UnpaidDays { get; set; }
        public List<LeaveDayDto> LeaveDays { get; set; } = new List<LeaveDayDto>();
        public string Reason { get; set; } = string.Empty;
        public LeaveStatus Status { get; set; }
        public DateTime AppliedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public Guid? DepartmentHeadId { get; set; }
        public Guid? HrPersonnelId { get; set; }
        public ApprovalInfoDto? DepartmentHeadApproval { get; set; }
        public ApprovalInfoDto? HrAcknowledgment { get; set; }
        public string ApprovedBy { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
    }

    public class LeaveDayDto
    {
        public DateTime Date { get; set; }
        public bool IsPaid { get; set; }
    }

    public class ApprovalInfoDto
    {
        public string ApprovedBy { get; set; } = string.Empty;
        public DateTime ApprovedDate { get; set; }
        public string? Comments { get; set; }
    }
} 