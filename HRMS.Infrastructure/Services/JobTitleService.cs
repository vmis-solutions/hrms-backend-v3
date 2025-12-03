using HRMS.Application.Common;
using HRMS.Application.DTOs.JobTitles;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.JobTitles;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class JobTitleService : IJobTitleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobTitleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JobTitleDto?> GetJobTitleByIdAsync(Guid id)
        {
            var jobTitle = await _unitOfWork.JobTitle.GetByIdAsync(id);
            if (jobTitle == null) return null;

            return MapJobTitle(jobTitle);
        }

        public async Task<IEnumerable<JobTitleDto>> GetAllJobTitlesAsync()
        {
            var jobTitles = await _unitOfWork.JobTitle.GetAllAsync();
            return jobTitles.Select(MapJobTitle);
        }

        public async Task<PagedResult<JobTitleDto>> GetJobTitlesByUserRoleAsync(Guid userId, string role, int pageNumber, int pageSize, string? searchTerm = null)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            IEnumerable<JobTitle> jobTitles = Enumerable.Empty<JobTitle>();
            int totalCount = 0;

            if (string.Equals(role, "HR_MANAGER", StringComparison.OrdinalIgnoreCase))
            {
                (jobTitles, totalCount) = await _unitOfWork.JobTitle.GetAllPaginatedAsync(pageNumber, pageSize, searchTerm);
            }
            else
            {
                (jobTitles, totalCount) = await _unitOfWork.JobTitle.GetJobTitlesByHrManagerIdPaginatedAsync(userId, pageNumber, pageSize, searchTerm);
            }

            return new PagedResult<JobTitleDto>
            {
                Items = jobTitles.Select(MapJobTitle),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<JobTitleDto> CreateJobTitleAsync(JobTitleCreateDto dto)
        {
            var jobTitle = new JobTitle
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                DepartmentId = dto.DepartmentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.JobTitle.AddAsync(jobTitle);
            await _unitOfWork.SaveChangesAsync();

            return MapJobTitle(jobTitle);
        }

        public async Task<JobTitleDto> UpdateJobTitleAsync(JobTitleUpdateDto dto)
        {
            var jobTitle = await _unitOfWork.JobTitle.GetByIdAsync(dto.Id);
            if (jobTitle == null) throw new Exception("Job title not found");

            jobTitle.Title = dto.Title;
            jobTitle.Description = dto.Description;
            jobTitle.DepartmentId = dto.DepartmentId;
            jobTitle.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.JobTitle.UpdateAsync(jobTitle);
            await _unitOfWork.SaveChangesAsync();

            return MapJobTitle(jobTitle);
        }

        public async Task DeleteJobTitleAsync(Guid id)
        {
            var jobTitle = await _unitOfWork.JobTitle.GetByIdAsync(id);
            if (jobTitle == null) throw new Exception("Job title not found");

            await _unitOfWork.JobTitle.DeleteAsync(jobTitle.Id);
            await _unitOfWork.SaveChangesAsync();
        }
        private static JobTitleDto MapJobTitle(JobTitle jobTitle)
        {
            return new JobTitleDto
            {
                Id = jobTitle.Id,
                Title = jobTitle.Title,
                Description = jobTitle.Description,
                DepartmentId = jobTitle.DepartmentId,
                DepartmentName = jobTitle.Department?.Name ?? string.Empty,
                CompanyId = jobTitle.Department?.CompanyId ?? Guid.Empty,
                CompanyName = jobTitle.Department?.Company?.Name ?? string.Empty
            };
        }
    }
}
