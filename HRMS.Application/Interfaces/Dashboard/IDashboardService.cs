using HRMS.Application.DTOs.Dashboard;
using System;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        Task<EmployeeChart> GetEmployeeChartAsync(DateTime dateFrom, DateTime dateTo);
    }
}
