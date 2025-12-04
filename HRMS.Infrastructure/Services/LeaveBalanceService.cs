using HRMS.Application.DTOs.LeaveBalances;
using HRMS.Application.Interfaces.LeaveBalances;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly ILeaveBalanceRepository _repository;

        public LeaveBalanceService(ILeaveBalanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<LeaveBalanceDto?> GetLeaveBalanceByIdAsync(Guid id)
        {
            var leaveBalance = await _repository.GetByIdAsync(id);
            if (leaveBalance == null) return null;

            return new LeaveBalanceDto
            {
                Id = leaveBalance.Id,
                EmployeeId = leaveBalance.EmployeeId,
                LeaveType = leaveBalance.LeaveType,
                Year = leaveBalance.Year,
                TotalDays = leaveBalance.TotalDays,
                UsedDays = leaveBalance.UsedDays,
                RemainingDays = leaveBalance.RemainingDays,
                EmployeeName = leaveBalance.Employee?.GetFullName()
            };
        }

        public async Task<IEnumerable<LeaveBalanceDto>> GetAllLeaveBalancesAsync()
        {
            var leaveBalances = await _repository.GetAllAsync();
            return leaveBalances.Select(lb => new LeaveBalanceDto
            {
                Id = lb.Id,
                EmployeeId = lb.EmployeeId,
                LeaveType = lb.LeaveType,
                Year = lb.Year,
                TotalDays = lb.TotalDays,
                UsedDays = lb.UsedDays,
                RemainingDays = lb.RemainingDays,
                EmployeeName = lb.Employee?.GetFullName()
            });
        }

        public async Task<LeaveBalanceDto?> GetLeaveBalanceByEmployeeIdAndYearAsync(Guid employeeId, int year)
        {
            var leaveBalance = await _repository.GetByEmployeeIdAndYearAsync(employeeId, year);
            if (leaveBalance == null) return null;

            return new LeaveBalanceDto
            {
                Id = leaveBalance.Id,
                EmployeeId = leaveBalance.EmployeeId,
                LeaveType = leaveBalance.LeaveType,
                Year = leaveBalance.Year,
                TotalDays = leaveBalance.TotalDays,
                UsedDays = leaveBalance.UsedDays,
                RemainingDays = leaveBalance.RemainingDays,
                EmployeeName = leaveBalance.Employee?.GetFullName()
            };
        }

        public async Task<IEnumerable<LeaveBalanceDto>> GetLeaveBalancesByEmployeeIdAsync(Guid employeeId)
        {
            var leaveBalances = await _repository.GetByEmployeeIdAsync(employeeId);
            return leaveBalances.Select(lb => new LeaveBalanceDto
            {
                Id = lb.Id,
                EmployeeId = lb.EmployeeId,
                LeaveType = lb.LeaveType,
                Year = lb.Year,
                TotalDays = lb.TotalDays,
                UsedDays = lb.UsedDays,
                RemainingDays = lb.RemainingDays,
                EmployeeName = lb.Employee?.GetFullName()
            });
        }

        public async Task<LeaveBalanceDto> CreateLeaveBalanceAsync(LeaveBalanceCreateDto dto)
        {
            var leaveBalance = new LeaveBalance
            {
                Id = Guid.NewGuid(),
                EmployeeId = dto.EmployeeId,
                LeaveType = dto.LeaveType,
                Year = dto.Year,
                TotalDays = dto.TotalDays,
                UsedDays = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(leaveBalance);

            return new LeaveBalanceDto
            {
                Id = leaveBalance.Id,
                EmployeeId = leaveBalance.EmployeeId,
                LeaveType = leaveBalance.LeaveType,
                Year = leaveBalance.Year,
                TotalDays = leaveBalance.TotalDays,
                UsedDays = leaveBalance.UsedDays,
                RemainingDays = leaveBalance.RemainingDays
            };
        }

        public async Task<LeaveBalanceDto> UpdateLeaveBalanceAsync(LeaveBalanceUpdateDto dto)
        {
            var leaveBalance = await _repository.GetByIdAsync(dto.Id);
            if (leaveBalance == null) throw new Exception("Leave balance not found");

            leaveBalance.LeaveType = dto.LeaveType;
            leaveBalance.Year = dto.Year;
            leaveBalance.TotalDays = dto.TotalDays;
            leaveBalance.UsedDays = dto.UsedDays;
            leaveBalance.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(leaveBalance);

            return new LeaveBalanceDto
            {
                Id = leaveBalance.Id,
                EmployeeId = leaveBalance.EmployeeId,
                LeaveType = leaveBalance.LeaveType,
                Year = leaveBalance.Year,
                TotalDays = leaveBalance.TotalDays,
                UsedDays = leaveBalance.UsedDays,
                RemainingDays = leaveBalance.RemainingDays
            };
        }

        public async Task DeleteLeaveBalanceAsync(Guid id)
        {
            var leaveBalance = await _repository.GetByIdAsync(id);
            if (leaveBalance == null) throw new Exception("Leave balance not found");

            await _repository.DeleteAsync(leaveBalance.Id);
        }
    }
}
