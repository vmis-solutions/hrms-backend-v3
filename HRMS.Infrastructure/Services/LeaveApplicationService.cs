using HRMS.Application.DTOs.LeaveApplications;
using HRMS.Application.Interfaces.LeaveApplications;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class LeaveApplicationService : ILeaveApplicationService
    {
        private readonly ILeaveApplicationRepository _repository;

        public LeaveApplicationService(ILeaveApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<LeaveApplicationDto?> GetLeaveApplicationByIdAsync(Guid id)
        {
            var leaveApplication = await _repository.GetByIdAsync(id);
            if (leaveApplication == null) return null;

            return new LeaveApplicationDto
            {
                Id = leaveApplication.Id,
                EmployeeId = leaveApplication.EmployeeId,
                LeaveType = leaveApplication.LeaveType,
                StartDate = leaveApplication.StartDate,
                EndDate = leaveApplication.EndDate,
                Reason = leaveApplication.Reason,
                Status = leaveApplication.Status,
                AppliedDate = leaveApplication.AppliedDate,
                ApprovedDate = leaveApplication.ApprovedDate,
                ApprovedBy = leaveApplication.ApprovedBy,
                Comments = leaveApplication.Comments,
                EmployeeName = leaveApplication.Employee?.GetFullName()
            };
        }

        public async Task<IEnumerable<LeaveApplicationDto>> GetAllLeaveApplicationsAsync()
        {
            var leaveApplications = await _repository.GetAllAsync();
            return leaveApplications.Select(la => new LeaveApplicationDto
            {
                Id = la.Id,
                EmployeeId = la.EmployeeId,
                LeaveType = la.LeaveType,
                StartDate = la.StartDate,
                EndDate = la.EndDate,
                Reason = la.Reason,
                Status = la.Status,
                AppliedDate = la.AppliedDate,
                ApprovedDate = la.ApprovedDate,
                ApprovedBy = la.ApprovedBy,
                Comments = la.Comments,
                EmployeeName = la.Employee?.GetFullName()
            });
        }

        public async Task<IEnumerable<LeaveApplicationDto>> GetLeaveApplicationsByEmployeeIdAsync(Guid employeeId)
        {
            var leaveApplications = await _repository.GetByEmployeeIdAsync(employeeId);
            return leaveApplications.Select(la => new LeaveApplicationDto
            {
                Id = la.Id,
                EmployeeId = la.EmployeeId,
                LeaveType = la.LeaveType,
                StartDate = la.StartDate,
                EndDate = la.EndDate,
                Reason = la.Reason,
                Status = la.Status,
                AppliedDate = la.AppliedDate,
                ApprovedDate = la.ApprovedDate,
                ApprovedBy = la.ApprovedBy,
                Comments = la.Comments,
                EmployeeName = la.Employee?.GetFullName()
            });
        }

        public async Task<LeaveApplicationDto> CreateLeaveApplicationAsync(LeaveApplicationCreateDto dto)
        {
            var leaveApplication = new LeaveApplication
            {
                Id = Guid.NewGuid(),
                EmployeeId = dto.EmployeeId,
                LeaveType = dto.LeaveType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Status = Domain.Enums.LeaveStatus.Pending,
                AppliedDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(leaveApplication);

            return new LeaveApplicationDto
            {
                Id = leaveApplication.Id,
                EmployeeId = leaveApplication.EmployeeId,
                LeaveType = leaveApplication.LeaveType,
                StartDate = leaveApplication.StartDate,
                EndDate = leaveApplication.EndDate,
                Reason = leaveApplication.Reason,
                Status = leaveApplication.Status,
                AppliedDate = leaveApplication.AppliedDate
            };
        }

        public async Task<LeaveApplicationDto> UpdateLeaveApplicationAsync(LeaveApplicationUpdateDto dto)
        {
            var leaveApplication = await _repository.GetByIdAsync(dto.Id);
            if (leaveApplication == null) throw new Exception("Leave application not found");

            leaveApplication.LeaveType = dto.LeaveType;
            leaveApplication.StartDate = dto.StartDate;
            leaveApplication.EndDate = dto.EndDate;
            leaveApplication.Reason = dto.Reason;
            leaveApplication.Status = dto.Status;
            leaveApplication.ApprovedDate = dto.ApprovedDate;
            leaveApplication.ApprovedBy = dto.ApprovedBy;
            leaveApplication.Comments = dto.Comments;
            leaveApplication.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(leaveApplication);

            return new LeaveApplicationDto
            {
                Id = leaveApplication.Id,
                EmployeeId = leaveApplication.EmployeeId,
                LeaveType = leaveApplication.LeaveType,
                StartDate = leaveApplication.StartDate,
                EndDate = leaveApplication.EndDate,
                Reason = leaveApplication.Reason,
                Status = leaveApplication.Status,
                AppliedDate = leaveApplication.AppliedDate,
                ApprovedDate = leaveApplication.ApprovedDate,
                ApprovedBy = leaveApplication.ApprovedBy,
                Comments = leaveApplication.Comments
            };
        }

        public async Task DeleteLeaveApplicationAsync(Guid id)
        {
            var leaveApplication = await _repository.GetByIdAsync(id);
            if (leaveApplication == null) throw new Exception("Leave application not found");

            await _repository.DeleteAsync(leaveApplication.Id);
        }
    }
}
