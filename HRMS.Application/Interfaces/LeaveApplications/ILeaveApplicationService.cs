using HRMS.Application.DTOs.LeaveApplications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.LeaveApplications
{
    public interface ILeaveApplicationService
    {
        Task<LeaveApplicationDto?> GetLeaveApplicationByIdAsync(Guid id);
        Task<IEnumerable<LeaveApplicationDto>> GetAllLeaveApplicationsAsync();
        Task<IEnumerable<LeaveApplicationDto>> GetLeaveApplicationsByEmployeeIdAsync(Guid employeeId);
        Task<LeaveApplicationDto> CreateLeaveApplicationAsync(LeaveApplicationCreateDto dto);
        Task<LeaveApplicationDto> UpdateLeaveApplicationAsync(LeaveApplicationUpdateDto dto);
        Task DeleteLeaveApplicationAsync(Guid id);
    }
}
