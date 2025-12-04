using HRMS.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        Task<EmployeeChart> GetEmployeeChartAsync(DateTime dateFrom, DateTime dateTo);
        Task<DashboardStatsResponse> GetStatsAsync();
        Task<List<RecentActivity>> GetRecentActivitiesAsync(int limit = 10);
        Task<List<UpcomingBirthdayResponse>> GetUpcomingBirthdaysAsync(int days = 30);
        Task<DashboardResponse> GetDashboardAsync(int activityLimit = 10, int birthdayDays = 30);
    }
}
