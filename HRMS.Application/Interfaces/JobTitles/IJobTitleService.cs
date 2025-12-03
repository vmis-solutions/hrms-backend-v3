using HRMS.Application.Common;
using HRMS.Application.DTOs.JobTitles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.JobTitles
{
    public interface IJobTitleService
    {
        Task<JobTitleDto?> GetJobTitleByIdAsync(Guid id);
        Task<IEnumerable<JobTitleDto>> GetAllJobTitlesAsync();
        Task<PagedResult<JobTitleDto>> GetJobTitlesByUserRoleAsync(Guid userId, string role, int pageNumber, int pageSize, string? searchTerm = null);
        Task<JobTitleDto> CreateJobTitleAsync(JobTitleCreateDto dto);
        Task<JobTitleDto> UpdateJobTitleAsync(JobTitleUpdateDto dto);
        Task DeleteJobTitleAsync(Guid id);
    }
}
