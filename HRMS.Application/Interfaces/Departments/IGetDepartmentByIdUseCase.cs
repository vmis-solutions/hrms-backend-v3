using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Departments
{
    public interface IGetDepartmentByIdUseCase
    {
        Task<DepartmentDto?> ExecuteAsync(Guid id);
    }
} 