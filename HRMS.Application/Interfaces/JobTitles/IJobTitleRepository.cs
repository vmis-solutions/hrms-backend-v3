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
        Task AddAsync(JobTitle jobTitle);
        Task UpdateAsync(JobTitle jobTitle);
        Task DeleteAsync(Guid id);
    }
} 