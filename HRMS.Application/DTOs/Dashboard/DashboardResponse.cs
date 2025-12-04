using System.Collections.Generic;

namespace HRMS.Application.DTOs.Dashboard
{
    public class DashboardResponse
    {
        public DashboardStatsResponse Stats { get; set; } = new();
        public List<RecentActivity> RecentActivities { get; set; } = new();
        public List<UpcomingBirthdayResponse> UpcomingBirthdays { get; set; } = new();
    }
}

