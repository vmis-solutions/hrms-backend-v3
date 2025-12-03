using HRMS.Application.Interfaces.Employees;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Employee?> GetByEmployeeNumberAsync(string employeeNumber)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
        }

        public async Task<Employee?> GetBySssNumberAsync(string sssNumber)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.SssNumber == sssNumber);
        }

        public async Task<Employee?> GetByPhilHealthNumberAsync(string philHealthNumber)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.PhilHealthNumber == philHealthNumber);
        }

        public async Task<Employee?> GetByPagIbigNumberAsync(string pagIbigNumber)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.PagIbigNumber == pagIbigNumber);
        }

        public async Task<Employee?> GetByTinAsync(string tin)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Tin == tin);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Company)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Employee> Employees, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            IQueryable<Employee> query = _context.Employees
                .Include(e => e.Company)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName);

            query = ApplySearchFilter(query, searchTerm);

            var totalCount = await query.CountAsync();
            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (employees, totalCount);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByHrManagerIdAsync(Guid hrManagerId)
        {
            // Get all department IDs where this employee is an HR Manager
            var departmentIds = await _context.DepartmentHrManagers
                .Where(dhm => dhm.EmployeeId == hrManagerId)
                .Select(dhm => dhm.DepartmentId)
                .ToListAsync();

            // Get all employees in those departments
            return await _context.Employees
                .Where(e => departmentIds.Contains(e.DepartmentId))
                .Include(e => e.Company)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Employee> Employees, int TotalCount)> GetEmployeesByHrManagerIdPaginatedAsync(Guid hrManagerId, int pageNumber, int pageSize, string? searchTerm = null)
        {
            var departmentIds = await _context.DepartmentHrManagers
                .Where(dhm => dhm.EmployeeId == hrManagerId)
                .Select(dhm => dhm.DepartmentId)
                .ToListAsync();

            IQueryable<Employee> query = _context.Employees
                .Where(e => departmentIds.Contains(e.DepartmentId))
                .Include(e => e.Company)
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName);

            query = ApplySearchFilter(query, searchTerm);

            var totalCount = await query.CountAsync();

            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (employees, totalCount);
        }

        private static IQueryable<Employee> ApplySearchFilter(IQueryable<Employee> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            var like = $"%{searchTerm.Trim()}%";

            return query.Where(e =>
                EF.Functions.Like(e.FirstName, like) ||
                EF.Functions.Like(e.LastName, like) ||
                EF.Functions.Like(e.MiddleName ?? string.Empty, like) ||
                EF.Functions.Like((e.FirstName + " " + e.LastName), like) ||
                EF.Functions.Like((e.LastName + ", " + e.FirstName), like) ||
                EF.Functions.Like(e.Department.Name ?? string.Empty, like) ||
                EF.Functions.Like(e.Company.Name ?? string.Empty, like) ||
                EF.Functions.Like(e.JobTitle.Title ?? string.Empty, like));
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
        }
    }
} 