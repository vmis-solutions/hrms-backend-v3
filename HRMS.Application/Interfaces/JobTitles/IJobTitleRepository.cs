using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.JobTitles
{
    public interface IJobTitleRepository
    {
        Task<JobTitle?> GetByIdAsync(Guid id);
        Task<IEnumerable<JobTitle>> GetAllAsync();
        Task<(IEnumerable<JobTitle> JobTitles, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<(IEnumerable<JobTitle> JobTitles, int TotalCount)> GetJobTitlesByHrManagerIdPaginatedAsync(Guid hrManagerId, int pageNumber, int pageSize, string? searchTerm = null);
        Task AddAsync(JobTitle jobTitle);
        Task UpdateAsync(JobTitle jobTitle);
        Task DeleteAsync(Guid id);
    }
} 