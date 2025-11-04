using HRMS.Application.Interfaces.JobTitles;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly ApplicationDbContext _context;
        public JobTitleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobTitle?> GetByIdAsync(Guid id)
        {
            return await _context.JobTitles.FindAsync(id);
        }

        public async Task<IEnumerable<JobTitle>> GetAllAsync()
        {
            return await _context.JobTitles.ToListAsync();
        }

        public async Task AddAsync(JobTitle jobTitle)
        {
            await _context.JobTitles.AddAsync(jobTitle);
        }

        public async Task UpdateAsync(JobTitle jobTitle)
        {
            _context.JobTitles.Update(jobTitle);
        }

        public async Task DeleteAsync(Guid id)
        {
            var jobTitle = await _context.JobTitles.FindAsync(id);
            if (jobTitle != null)
            {
                _context.JobTitles.Remove(jobTitle);
            }
        }
    }
} 