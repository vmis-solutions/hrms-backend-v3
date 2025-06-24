using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : DbContext, IDBContext
    {
        public DbSet<ApprovalInfo> ApprovalInfos => Set<ApprovalInfo>();

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Company> Companies => Set<Company>();

        public DbSet<Department> Departments => Set<Department>();

        public DbSet<JobTitle> JobTitles => Set<JobTitle>();

        public DbSet<LeaveApplication> LeaveApplications => Set<LeaveApplication>();

        public DbSet<LeaveDay> LeaveDays => Set<LeaveDay>();

        public DbSet<Attendance> Attendances => Set<Attendance>();

        public DbSet<Payroll> Payrolls => Set<Payroll>();

        public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();
    }
}
