using HRMS.Application.DTOs;
using HRMS.Application.NewFolder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Departments
{
    public interface IDepartmentFacade
    {
        Task<DepartmentDto?> GetByIdAsync(Guid id);
        Task<List<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto> CreateAsync(DepartmentCreateDto dto);
        Task<DepartmentDto?> UpdateAsync(DepartmentUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
} 