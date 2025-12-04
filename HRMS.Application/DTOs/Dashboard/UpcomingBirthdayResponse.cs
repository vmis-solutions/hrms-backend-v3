using System;

namespace HRMS.Application.DTOs.Dashboard
{
    public class UpcomingBirthdayResponse
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string? EmployeeAvatar { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime NextBirthday { get; set; }
        public int DaysUntil { get; set; }
        public int Age { get; set; }
        public bool IsToday { get; set; }
        public bool IsThisWeek { get; set; }
    }
}

