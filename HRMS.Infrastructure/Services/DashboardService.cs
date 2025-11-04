using HRMS.Application.DTOs.Dashboard;
using HRMS.Application.Interfaces.Dashboard;
using HRMS.Application.Interfaces.Employees;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public DashboardService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeChart> GetEmployeeChartAsync(DateTime dateFrom, DateTime dateTo)
        {
            var employees = await _employeeRepository.GetAllAsync();
            
            var employeesInRange = employees.Where(e => e.DateHired >= dateFrom && e.DateHired <= dateTo);
            
            var chart = new EmployeeChart
            {
                TotalEmployees = employees.Count(),
                NewHires = employeesInRange.Count(),
                ActiveEmployees = employees.Count(e => e.EmploymentStatus == Domain.Enums.EmploymentStatus.Active),
                InactiveEmployees = employees.Count(e => e.EmploymentStatus == Domain.Enums.EmploymentStatus.Inactive),
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            return chart;
        }
    }
}
