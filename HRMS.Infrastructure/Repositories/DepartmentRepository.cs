using HRMS.Application.Departments;
using HRMS.Application.Interfaces.Companies;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> GetByIdAsync(Guid id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
        }

        public async Task DeleteAsync(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
        }
    }
} 