using HRMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class LeaveApplication : BaseAuditable
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public LeaveType LeaveType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int TotalDays { get; set; }

        [Required]
        public int PaidDays { get; set; }

        [Required]
        public int UnpaidDays { get; set; }

        [Required]
        public List<LeaveDay> LeaveDays { get; set; } = new List<LeaveDay>();

        [Required]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public LeaveStatus Status { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public string ApprovedBy { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;

        // Approval workflow with proper foreign keys
        public Guid? DepartmentHeadId { get; set; }
        public Guid? HrPersonnelId { get; set; }

        public ApprovalInfo? DepartmentHeadApproval { get; set; }
        public ApprovalInfo? HrAcknowledgment { get; set; }

        // Navigation properties
        public Employee Employee { get; set; } = null!;
        public Employee? DepartmentHead { get; set; }
        public Employee? HrPersonnel { get; set; }

        // Domain methods
        public bool CanBeApprovedBy(Guid employeeId) =>
            Status == LeaveStatus.Pending && DepartmentHeadId == employeeId;

        public void ApproveByDepartmentHead(string approvedBy, string? comments = null)
        {
            if (Status != LeaveStatus.Pending)
                throw new InvalidOperationException("Leave can only be approved when pending");

            Status = LeaveStatus.Approved_by_Department;
            DepartmentHeadApproval = new ApprovalInfo
            {
                ApprovedBy = approvedBy,
                ApprovedDate = DateTime.UtcNow,
                Comments = comments
            };
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
