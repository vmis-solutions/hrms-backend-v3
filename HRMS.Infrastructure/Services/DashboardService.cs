using HRMS.Application.DTOs.Dashboard;
using HRMS.Application.Interfaces.Dashboard;
using HRMS.Application.Interfaces.Employees;
using HRMS.Domain.Enums;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ApplicationDbContext _context;

        public DashboardService(IEmployeeRepository employeeRepository, ApplicationDbContext context)
        {
            _employeeRepository = employeeRepository;
            _context = context;
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

        public async Task<DashboardStatsResponse> GetStatsAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employeeList = employees.ToList();

            // Current stats
            var totalEmployees = employeeList.Count;
            var regularEmployees = employeeList.Count(e => e.EmploymentStatus == EmploymentStatus.Regular);
            var probationaryEmployees = employeeList.Count(e => e.EmploymentStatus == EmploymentStatus.Probationary);
            var contractualEmployees = employeeList.Count(e => e.EmploymentStatus == EmploymentStatus.Contractual);
            var projectBasedEmployees = employeeList.Count(e => e.EmploymentStatus == EmploymentStatus.ProjectBased);
            var resignedEmployees = employeeList.Count(e => e.EmploymentStatus == EmploymentStatus.Resigned);
            var terminatedEmployees = employeeList.Count(e => e.EmploymentStatus == EmploymentStatus.Terminated);

            // Previous month stats for trends
            // We approximate by counting employees that existed at the end of previous month
            var now = DateTime.UtcNow;
            var firstDayOfCurrentMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            
            // Employees that existed before current month (approximation of previous month end)
            var previousMonthEmployees = employeeList.Where(e => e.CreatedAt < firstDayOfCurrentMonth).ToList();
            var previousTotalEmployees = previousMonthEmployees.Count;
            var previousRegularEmployees = previousMonthEmployees.Count(e => e.EmploymentStatus == EmploymentStatus.Regular);
            var previousProbationaryEmployees = previousMonthEmployees.Count(e => e.EmploymentStatus == EmploymentStatus.Probationary);
            var previousContractualEmployees = previousMonthEmployees.Count(e => e.EmploymentStatus == EmploymentStatus.Contractual);

            // Calculate trends
            var totalEmployeesTrend = CalculateTrend(totalEmployees, previousTotalEmployees);
            var regularEmployeesTrend = CalculateTrend(regularEmployees, previousRegularEmployees);
            var probationaryEmployeesTrend = CalculateTrend(probationaryEmployees, previousProbationaryEmployees);
            var contractualEmployeesTrend = CalculateTrend(contractualEmployees, previousContractualEmployees);

            return new DashboardStatsResponse
            {
                TotalEmployees = totalEmployees,
                RegularEmployees = regularEmployees,
                ProbationaryEmployees = probationaryEmployees,
                ContractualEmployees = contractualEmployees,
                ProjectBasedEmployees = projectBasedEmployees,
                ResignedEmployees = resignedEmployees,
                TerminatedEmployees = terminatedEmployees,
                TotalEmployeesTrend = totalEmployeesTrend,
                RegularEmployeesTrend = regularEmployeesTrend,
                ProbationaryEmployeesTrend = probationaryEmployeesTrend,
                ContractualEmployeesTrend = contractualEmployeesTrend
            };
        }

        public async Task<List<RecentActivity>> GetRecentActivitiesAsync(int limit = 10)
        {
            var activities = new List<RecentActivity>();

            // Get recent employee hires (created employees)
            var recentEmployees = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .OrderByDescending(e => e.CreatedAt)
                .Take(limit)
                .ToListAsync();

            foreach (var employee in recentEmployees)
            {
                activities.Add(new RecentActivity
                {
                    Id = employee.Id,
                    Type = ActivityType.Hire,
                    EmployeeId = employee.Id,
                    Action = "New employee onboarded",
                    Status = employee.EmploymentStatus.ToString(),
                    Timestamp = employee.CreatedAt,
                    CreatedAt = employee.CreatedAt,
                    EmployeeName = employee.GetFullName(),
                    EmployeePosition = employee.JobTitle?.Title ?? string.Empty,
                    EmployeeAvatar = employee.Avatar
                });
            }

            // Get recent leave applications
            var recentLeaves = await _context.LeaveApplications
                .Include(la => la.Employee)
                    .ThenInclude(e => e.JobTitle)
                .OrderByDescending(la => la.CreatedAt)
                .Take(limit)
                .ToListAsync();

            foreach (var leave in recentLeaves)
            {
                activities.Add(new RecentActivity
                {
                    Id = leave.Id,
                    Type = ActivityType.Leave,
                    EmployeeId = leave.EmployeeId,
                    Action = $"Leave application submitted - {leave.LeaveType}",
                    Status = leave.Status.ToString(),
                    Timestamp = leave.CreatedAt,
                    CreatedAt = leave.CreatedAt,
                    EmployeeName = leave.Employee.GetFullName(),
                    EmployeePosition = leave.Employee.JobTitle?.Title ?? string.Empty,
                    EmployeeAvatar = leave.Employee.Avatar
                });
            }

            // Get recent employee document uploads
            var recentDocs = await _context.EmployeeDocs
                .Include(ed => ed.Employee)
                    .ThenInclude(e => e.JobTitle)
                .OrderByDescending(ed => ed.CreatedAt)
                .Take(limit)
                .ToListAsync();

            foreach (var doc in recentDocs)
            {
                activities.Add(new RecentActivity
                {
                    Id = doc.Id,
                    Type = ActivityType.Document,
                    EmployeeId = doc.EmployeeId,
                    Action = $"Document uploaded - {doc.DocumentName}",
                    Status = null,
                    Timestamp = doc.CreatedAt,
                    CreatedAt = doc.CreatedAt,
                    EmployeeName = doc.Employee.GetFullName(),
                    EmployeePosition = doc.Employee.JobTitle?.Title ?? string.Empty,
                    EmployeeAvatar = doc.Employee.Avatar
                });
            }

            // Get recent employee status changes (updated employees where status might have changed)
            // Note: This is a simplified approach. In a real system, you'd track status changes explicitly
            var recentUpdates = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Where(e => e.UpdatedAt != e.CreatedAt && e.UpdatedAt > DateTime.UtcNow.AddDays(-30))
                .OrderByDescending(e => e.UpdatedAt)
                .Take(limit)
                .ToListAsync();

            foreach (var employee in recentUpdates)
            {
                // Only add if it's not already in the list as a hire
                if (!activities.Any(a => a.Id == employee.Id && a.Type == ActivityType.Hire))
                {
                    activities.Add(new RecentActivity
                    {
                        Id = employee.Id,
                        Type = ActivityType.ProfileUpdate,
                        EmployeeId = employee.Id,
                        Action = "Employee profile updated",
                        Status = employee.EmploymentStatus.ToString(),
                        Timestamp = employee.UpdatedAt,
                        CreatedAt = employee.UpdatedAt,
                        EmployeeName = employee.GetFullName(),
                        EmployeePosition = employee.JobTitle?.Title ?? string.Empty,
                        EmployeeAvatar = employee.Avatar
                    });
                }
            }

            // Sort by timestamp descending and take the limit
            return activities
                .OrderByDescending(a => a.Timestamp)
                .Take(limit)
                .ToList();
        }

        public async Task<List<UpcomingBirthdayResponse>> GetUpcomingBirthdaysAsync(int days = 30)
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Where(e => e.BirthDate != default)
                .ToListAsync();

            var today = DateTime.UtcNow.Date;
            var endDate = today.AddDays(days);
            var upcomingBirthdays = new List<UpcomingBirthdayResponse>();

            foreach (var employee in employees)
            {
                var birthDate = employee.BirthDate.Date;
                var thisYearBirthday = new DateTime(today.Year, birthDate.Month, birthDate.Day);
                var nextBirthday = thisYearBirthday < today 
                    ? new DateTime(today.Year + 1, birthDate.Month, birthDate.Day)
                    : thisYearBirthday;

                if (nextBirthday <= endDate)
                {
                    var daysUntil = (nextBirthday - today).Days;
                    var age = today.Year - birthDate.Year;
                    if (birthDate.Date > today.AddYears(-age)) age--;

                    upcomingBirthdays.Add(new UpcomingBirthdayResponse
                    {
                        EmployeeId = employee.Id,
                        EmployeeName = employee.GetFullName(),
                        EmployeeAvatar = employee.Avatar,
                        JobTitle = employee.JobTitle?.Title ?? string.Empty,
                        Department = employee.Department?.Name ?? string.Empty,
                        BirthDate = birthDate,
                        NextBirthday = nextBirthday,
                        DaysUntil = daysUntil,
                        Age = age + 1, // Age they'll be on their birthday
                        IsToday = daysUntil == 0,
                        IsThisWeek = daysUntil >= 0 && daysUntil <= 7
                    });
                }
            }

            return upcomingBirthdays
                .OrderBy(b => b.DaysUntil)
                .ThenBy(b => b.EmployeeName)
                .ToList();
        }

        public async Task<DashboardResponse> GetDashboardAsync(int activityLimit = 10, int birthdayDays = 30)
        {
            var stats = await GetStatsAsync();
            var activities = await GetRecentActivitiesAsync(activityLimit);
            var birthdays = await GetUpcomingBirthdaysAsync(birthdayDays);

            return new DashboardResponse
            {
                Stats = stats,
                RecentActivities = activities,
                UpcomingBirthdays = birthdays
            };
        }

        private static DashboardTrend CalculateTrend(int current, int previous)
        {
            var change = current - previous;
            var isIncrease = change > 0;
            var description = change >= 0 
                ? $"+{change} from last month" 
                : $"{change} from last month";

            return new DashboardTrend
            {
                Change = change,
                IsIncrease = isIncrease,
                Description = description
            };
        }
    }
}
