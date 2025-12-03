using HRMS.Application.DTOs.Departments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.DepartmentHrManagers
{
    public interface IDepartmentHrManagerService
    {
        Task<List<DepartmentHrManagerDto>> GetByDepartmentIdAsync(Guid departmentId);
        Task<bool> DeleteAsync(Guid id);
    }
}

