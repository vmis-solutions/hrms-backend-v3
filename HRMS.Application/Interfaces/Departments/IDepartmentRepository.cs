using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Departments
{
    public interface IDepartmentRepository : IGeneric<Department>
    {
        Task<Department?> GetByIdWithHrManagersAsync(Guid id);
        Task<List<DepartmentHrManager>> GetHrManagersByDepartmentIdAsync(Guid departmentId);
        Task<DepartmentHrManager?> GetHrManagerByIdAsync(Guid id);
        Task<DepartmentHrManager?> GetHrManagerByDepartmentAndEmployeeAsync(Guid departmentId, Guid employeeId);
        Task AddHrManagerAsync(DepartmentHrManager departmentHrManager);
        Task UpdateHrManagerAsync(DepartmentHrManager departmentHrManager);
        Task RemoveHrManagerAsync(Guid id);
        Task<bool> HrManagerExistsAsync(Guid departmentId, Guid employeeId);
        Task<List<Department>> GetDepartmentsManagedByUserAsync(string userId);
    }
} 