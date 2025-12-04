using HRMS.Application.DTOs.Departments;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.DepartmentHrManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class DepartmentHrManagerService : IDepartmentHrManagerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentHrManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DepartmentHrManagerDto>> GetByDepartmentIdAsync(Guid departmentId)
        {
            var hrManagers = await _unitOfWork.Department.GetHrManagersByDepartmentIdAsync(departmentId);
            return hrManagers.Select(dhm => new DepartmentHrManagerDto
            {
                Id = dhm.Id,
                DepartmentId = dhm.DepartmentId,
                EmployeeId = dhm.EmployeeId,
                EmployeeName = dhm.Employee != null ? $"{dhm.Employee.FirstName} {dhm.Employee.LastName}" : string.Empty,
                EmployeeEmail = dhm.Employee?.Email ?? string.Empty,
                EmployeeNumber = dhm.Employee?.EmployeeNumber ?? string.Empty,
                AssignedAt = dhm.CreatedAt
            }).ToList();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var hrManager = await _unitOfWork.Department.GetHrManagerByIdAsync(id);
            if (hrManager == null) return false;

            await _unitOfWork.Department.RemoveHrManagerAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

