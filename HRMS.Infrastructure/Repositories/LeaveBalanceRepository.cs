using HRMS.Application.Interfaces.LeaveBalances;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaveBalanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveBalance?> GetByIdAsync(Guid id)
        {
            return await _context.LeaveBalances.FindAsync(id);
        }

        public async Task<IEnumerable<LeaveBalance>> GetAllAsync()
        {
            return await _context.LeaveBalances.ToListAsync();
        }

        public async Task<LeaveBalance?> GetByEmployeeIdAndYearAsync(Guid employeeId, int year)
        {
            return await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId && lb.Year == year);
        }

        public async Task<IEnumerable<LeaveBalance>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.LeaveBalances
                .Where(lb => lb.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task AddAsync(LeaveBalance leaveBalance)
        {
            await _context.LeaveBalances.AddAsync(leaveBalance);
        }

        public async Task UpdateAsync(LeaveBalance leaveBalance)
        {
            _context.LeaveBalances.Update(leaveBalance);
        }

        public async Task DeleteAsync(Guid id)
        {
            var leaveBalance = await _context.LeaveBalances.FindAsync(id);
            if (leaveBalance != null)
            {
                _context.LeaveBalances.Remove(leaveBalance);
            }
        }
    }
} 