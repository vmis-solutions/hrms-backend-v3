using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces
{
    public interface IDBContext
    {
        public DbSet<ApprovalInfo> ApprovalInfos { get; }
        public DbSet<Employee> Employees { get; }
        public DbSet<Company> Companies { get; }
        public DbSet<Department> Departments { get; }
        public DbSet<JobTitle> JobTitles { get; }
        public DbSet<LeaveApplication> LeaveApplications { get; }
        public DbSet<Attendance> Attendances { get; }
        public DbSet<Payroll> Payrolls { get; }
        public DbSet<LeaveBalance> LeaveBalances { get; }
    }
}
