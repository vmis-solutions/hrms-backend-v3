using HRMS.Application.Interfaces.LeaveApplications;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class LeaveApplicationRepository : ILeaveApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaveApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveApplication?> GetByIdAsync(Guid id)
        {
            return await _context.LeaveApplications.FindAsync(id);
        }

        public async Task<IEnumerable<LeaveApplication>> GetAllAsync()
        {
            return await _context.LeaveApplications.ToListAsync();
        }

        public async Task<IEnumerable<LeaveApplication>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.LeaveApplications
                .Where(la => la.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task AddAsync(LeaveApplication leaveApplication)
        {
            await _context.LeaveApplications.AddAsync(leaveApplication);
        }

        public async Task UpdateAsync(LeaveApplication leaveApplication)
        {
            _context.LeaveApplications.Update(leaveApplication);
        }

        public async Task DeleteAsync(Guid id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }
        }
    }
} 