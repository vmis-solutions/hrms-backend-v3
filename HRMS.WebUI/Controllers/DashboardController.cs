using HRMS.Application.DTOs.Dashboard;
using HRMS.Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class DashboardController : BaseController<DashboardController>
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
            : base(logger)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("employee-chart")]
        public async Task<IActionResult> GetEmployeeChart([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var chart = await _dashboardService.GetEmployeeChartAsync(dateFrom, dateTo);
            return CreateResponse(200, "Success", chart);
        }

        /// <summary>
        /// Get dashboard statistics with trends
        /// </summary>
        [HttpGet("Stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _dashboardService.GetStatsAsync();
            return CreateResponse(200, "Dashboard statistics retrieved successfully", stats);
        }

        /// <summary>
        /// Get recent activities
        /// </summary>
        [HttpGet("RecentActivities")]
        public async Task<IActionResult> GetRecentActivities([FromQuery] int limit = 10)
        {
            var activities = await _dashboardService.GetRecentActivitiesAsync(limit);
            return CreateResponse(200, "Recent activities retrieved successfully", activities);
        }

        /// <summary>
        /// Get upcoming birthdays
        /// </summary>
        [HttpGet("UpcomingBirthdays")]
        public async Task<IActionResult> GetUpcomingBirthdays([FromQuery] int days = 30)
        {
            var birthdays = await _dashboardService.GetUpcomingBirthdaysAsync(days);
            return CreateResponse(200, "Upcoming birthdays retrieved successfully", birthdays);
        }

        /// <summary>
        /// Get complete dashboard data (stats, activities, and birthdays)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboard([FromQuery] int activityLimit = 10, [FromQuery] int birthdayDays = 30)
        {
            var dashboard = await _dashboardService.GetDashboardAsync(activityLimit, birthdayDays);
            return CreateResponse(200, "Dashboard data retrieved successfully", dashboard);
        }
    }
} 