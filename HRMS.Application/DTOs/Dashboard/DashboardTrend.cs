namespace HRMS.Application.DTOs.Dashboard
{
    public class DashboardTrend
    {
        public int Change { get; set; } // Positive or negative change
        public bool IsIncrease { get; set; }
        public string Description { get; set; } = string.Empty; // e.g., "+12 from last month"
    }
}

