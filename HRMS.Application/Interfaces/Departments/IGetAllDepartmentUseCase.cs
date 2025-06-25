using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Departments
{
    public interface IGetAllDepartmentUseCase
    {
        Task<List<DepartmentDto>> ExecuteAsync();
    }
} 