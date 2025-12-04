namespace HRMS.Application.DTOs.Dashboard
{
    public class DashboardStatsResponse
    {
        public int TotalEmployees { get; set; }
        public int RegularEmployees { get; set; }
        public int ProbationaryEmployees { get; set; }
        public int ContractualEmployees { get; set; }
        public int ProjectBasedEmployees { get; set; }
        public int ResignedEmployees { get; set; }
        public int TerminatedEmployees { get; set; }

        // Trend data (comparison with previous month)
        public DashboardTrend TotalEmployeesTrend { get; set; } = new();
        public DashboardTrend RegularEmployeesTrend { get; set; } = new();
        public DashboardTrend ProbationaryEmployeesTrend { get; set; } = new();
        public DashboardTrend ContractualEmployeesTrend { get; set; } = new();
    }
}

