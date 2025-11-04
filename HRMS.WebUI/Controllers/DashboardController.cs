using HRMS.Application.DTOs.Dashboard;
using HRMS.Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
    }
} 