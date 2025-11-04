using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.LeaveBalances
{
    public interface ILeaveBalanceRepository
    {
        Task<LeaveBalance?> GetByIdAsync(Guid id);
        Task<IEnumerable<LeaveBalance>> GetAllAsync();
        Task<LeaveBalance?> GetByEmployeeIdAndYearAsync(Guid employeeId, int year);
        Task<IEnumerable<LeaveBalance>> GetByEmployeeIdAsync(Guid employeeId);
        Task AddAsync(LeaveBalance leaveBalance);
        Task UpdateAsync(LeaveBalance leaveBalance);
        Task DeleteAsync(Guid id);
    }
} 