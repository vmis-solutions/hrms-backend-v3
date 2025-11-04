using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.LeaveApplications
{
    public interface ILeaveApplicationRepository
    {
        Task<LeaveApplication?> GetByIdAsync(Guid id);
        Task<IEnumerable<LeaveApplication>> GetAllAsync();
        Task<IEnumerable<LeaveApplication>> GetByEmployeeIdAsync(Guid employeeId);
        Task AddAsync(LeaveApplication leaveApplication);
        Task UpdateAsync(LeaveApplication leaveApplication);
        Task DeleteAsync(Guid id);
    }
} 