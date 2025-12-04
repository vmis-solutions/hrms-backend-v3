using HRMS.Application.Interfaces.JobTitles;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<JobTitle?> GetByIdAsync(Guid id) =>
            await _context.JobTitles
                .Include(jt => jt.Department)
                    .ThenInclude(d => d.Company)
                .FirstOrDefaultAsync(jt => jt.Id == id);

        public async Task<IEnumerable<JobTitle>> GetAllAsync() =>
            await _context.JobTitles
                .Include(jt => jt.Department)
                    .ThenInclude(d => d.Company)
                .AsNoTracking()
                .ToListAsync();

        public async Task<(IEnumerable<JobTitle> JobTitles, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var query = BuildBaseQuery();
            query = ApplySearchFilter(query, searchTerm);

            var totalCount = await query.CountAsync();
            var jobTitles = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (jobTitles, totalCount);
        }

        public async Task<(IEnumerable<JobTitle> JobTitles, int TotalCount)> GetJobTitlesByHrManagerIdPaginatedAsync(Guid hrManagerId, int pageNumber, int pageSize, string? searchTerm = null)
        {
            var departmentIds = await _context.DepartmentHrManagers
                .Where(dhm => dhm.EmployeeId == hrManagerId)
                .Select(dhm => dhm.DepartmentId)
                .ToListAsync();

            if (!departmentIds.Any())
            {
                return (Enumerable.Empty<JobTitle>(), 0);
            }

            var query = BuildBaseQuery()
                .Where(jt => departmentIds.Contains(jt.DepartmentId));

            query = ApplySearchFilter(query, searchTerm);

            var totalCount = await query.CountAsync();
            var jobTitles = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (jobTitles, totalCount);
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

        private IQueryable<JobTitle> BuildBaseQuery()
        {
            return _context.JobTitles
                .Include(jt => jt.Department)
                    .ThenInclude(d => d.Company)
                .AsNoTracking()
                .OrderBy(jt => jt.Title);
        }

        private static IQueryable<JobTitle> ApplySearchFilter(IQueryable<JobTitle> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            var like = $"%{searchTerm.Trim()}%";

            return query.Where(jt =>
                EF.Functions.Like(jt.Title, like) ||
                EF.Functions.Like(jt.Department.Name ?? string.Empty, like) ||
                EF.Functions.Like(jt.Department.Company != null ? jt.Department.Company.Name : string.Empty, like));
        }
    }
} 