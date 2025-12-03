using System;

namespace HRMS.Application.DTOs.Dashboard
{
    public class RecentActivity
    {
        public Guid Id { get; set; }
        public ActivityType Type { get; set; }
        public Guid EmployeeId { get; set; }
        public string Action { get; set; } = string.Empty; // e.g., "New employee onboarded"
        public string? Status { get; set; } // Optional, for status changes
        public DateTime Timestamp { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeePosition { get; set; } = string.Empty;
        public string? EmployeeAvatar { get; set; }
    }
}

