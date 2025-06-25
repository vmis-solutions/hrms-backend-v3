using HRMS.Application.DTOs;
using HRMS.Application.NewFolder;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Departments
{
    public interface ICreateDepartmentUseCase
    {
        Task<DepartmentDto> ExecuteAsync(DepartmentCreateDto dto);
    }
} 