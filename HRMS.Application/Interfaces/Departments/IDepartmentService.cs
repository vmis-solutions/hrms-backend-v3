using HRMS.Application.DTOs.Departments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Departments
{
    public interface IDepartmentService
    {
        Task<DepartmentDto?> GetByIdAsync(Guid id);
        Task<List<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto> CreateAsync(DepartmentCreateDto dto);
        Task<DepartmentDto?> UpdateAsync(DepartmentUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
