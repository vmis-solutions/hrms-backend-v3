using HRMS.Application.DTOs.EmployeeDocs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.EmployeeDocs
{
    public interface IEmployeeDocService
    {
        Task<IEnumerable<EmployeeDocDto>> GetAllEmployeeDocsAsync();
        Task<EmployeeDocDto?> GetEmployeeDocByIdAsync(Guid id);
        Task<IEnumerable<EmployeeDocDto>> GetEmployeeDocsByEmployeeIdAsync(Guid employeeId);
        Task<EmployeeDocDto> CreateEmployeeDocAsync(EmployeeDocCreateDto dto);
        Task<EmployeeDocDto?> UpdateEmployeeDocAsync(EmployeeDocUpdateDto dto);
        Task DeleteEmployeeDocAsync(Guid id);
    }
}
