using HRMS.Application.DTOs.LeaveBalances;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.LeaveBalances
{
    public interface ILeaveBalanceService
    {
        Task<LeaveBalanceDto?> GetLeaveBalanceByIdAsync(Guid id);
        Task<IEnumerable<LeaveBalanceDto>> GetAllLeaveBalancesAsync();
        Task<LeaveBalanceDto?> GetLeaveBalanceByEmployeeIdAndYearAsync(Guid employeeId, int year);
        Task<IEnumerable<LeaveBalanceDto>> GetLeaveBalancesByEmployeeIdAsync(Guid employeeId);
        Task<LeaveBalanceDto> CreateLeaveBalanceAsync(LeaveBalanceCreateDto dto);
        Task<LeaveBalanceDto> UpdateLeaveBalanceAsync(LeaveBalanceUpdateDto dto);
        Task DeleteLeaveBalanceAsync(Guid id);
    }
}
