using ed = HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.EmployeeDocs
{
    public interface IEmployeeDocRepository
    {
        Task<ed.EmployeeDocs?> GetByIdAsync(Guid id);
        Task<IEnumerable<ed.EmployeeDocs>> GetAllAsync();
        Task<IEnumerable<ed.EmployeeDocs>> GetByEmployeeIdAsync(Guid employeeId);
        Task AddAsync(ed.EmployeeDocs employeeDoc);
        Task UpdateAsync(ed.EmployeeDocs employeeDoc);
        Task DeleteAsync(Guid id);
    }
} 