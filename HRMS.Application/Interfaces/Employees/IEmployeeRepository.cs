using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Employees
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(Guid id);
        Task<Employee?> GetByEmailAsync(string email);
        Task<Employee?> GetByEmployeeNumberAsync(string employeeNumber);
        Task<Employee?> GetBySssNumberAsync(string sssNumber);
        Task<Employee?> GetByPhilHealthNumberAsync(string philHealthNumber);
        Task<Employee?> GetByPagIbigNumberAsync(string pagIbigNumber);
        Task<Employee?> GetByTinAsync(string tin);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<(IEnumerable<Employee> Employees, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<IEnumerable<Employee>> GetEmployeesByHrManagerIdAsync(Guid hrManagerId);
        Task<(IEnumerable<Employee> Employees, int TotalCount)> GetEmployeesByHrManagerIdPaginatedAsync(Guid hrManagerId, int pageNumber, int pageSize, string? searchTerm = null);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Guid id);
    }
} 