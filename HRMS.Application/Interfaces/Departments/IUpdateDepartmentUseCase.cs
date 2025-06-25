using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Departments
{
    public interface IUpdateDepartmentUseCase
    {
        Task<DepartmentDto?> ExecuteAsync(DepartmentUpdateDto dto);
    }
} 