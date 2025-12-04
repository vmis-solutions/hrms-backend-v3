using HRMS.Application.Interfaces.EmployeeDocs;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeDocRepository : IEmployeeDocRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeDocRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeDocs?> GetByIdAsync(Guid id)
        {
            return await _context.EmployeeDocs.FindAsync(id);
        }

        public async Task<IEnumerable<EmployeeDocs>> GetAllAsync()
        {
            return await _context.EmployeeDocs.ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDocs>> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeDocs
                .Where(doc => doc.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task AddAsync(EmployeeDocs employeeDoc)
        {
            await _context.EmployeeDocs.AddAsync(employeeDoc);
        }

        public async Task UpdateAsync(EmployeeDocs employeeDoc)
        {
            _context.EmployeeDocs.Update(employeeDoc);
        }

        public async Task DeleteAsync(Guid id)
        {
            var employeeDoc = await _context.EmployeeDocs.FindAsync(id);
            if (employeeDoc != null)
            {
                _context.EmployeeDocs.Remove(employeeDoc);
            }
        }
    }
} 