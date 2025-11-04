using HRMS.Application.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Employees
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto dto);
        Task<EmployeeDto> UpdateEmployeeAsync(EmployeeUpdateDto dto);
        Task DeleteEmployeeAsync(Guid id);
    }
}
