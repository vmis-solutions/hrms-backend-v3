using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
        }

        public async Task DeleteAsync(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
            }
        }

        public async Task<Company?> FindByNameAsync(string name)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

    }
} 