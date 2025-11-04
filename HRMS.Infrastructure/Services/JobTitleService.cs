using HRMS.Application.DTOs.JobTitles;
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
        private readonly IJobTitleRepository _repository;

        public JobTitleService(IJobTitleRepository repository)
        {
            _repository = repository;
        }

        public async Task<JobTitleDto?> GetJobTitleByIdAsync(Guid id)
        {
            var jobTitle = await _repository.GetByIdAsync(id);
            if (jobTitle == null) return null;

            return new JobTitleDto
            {
                Id = jobTitle.Id,
                Title = jobTitle.Title,
                Description = jobTitle.Description,
                DepartmentId = jobTitle.DepartmentId,
                DepartmentName = jobTitle.Department?.Name
            };
        }

        public async Task<IEnumerable<JobTitleDto>> GetAllJobTitlesAsync()
        {
            var jobTitles = await _repository.GetAllAsync();
            return jobTitles.Select(jt => new JobTitleDto
            {
                Id = jt.Id,
                Title = jt.Title,
                Description = jt.Description,
                DepartmentId = jt.DepartmentId,
                DepartmentName = jt.Department?.Name
            });
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

            await _repository.AddAsync(jobTitle);

            return new JobTitleDto
            {
                Id = jobTitle.Id,
                Title = jobTitle.Title,
                Description = jobTitle.Description,
                DepartmentId = jobTitle.DepartmentId
            };
        }

        public async Task<JobTitleDto> UpdateJobTitleAsync(JobTitleUpdateDto dto)
        {
            var jobTitle = await _repository.GetByIdAsync(dto.Id);
            if (jobTitle == null) throw new Exception("Job title not found");

            jobTitle.Title = dto.Title;
            jobTitle.Description = dto.Description;
            jobTitle.DepartmentId = dto.DepartmentId;
            jobTitle.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(jobTitle);

            return new JobTitleDto
            {
                Id = jobTitle.Id,
                Title = jobTitle.Title,
                Description = jobTitle.Description,
                DepartmentId = jobTitle.DepartmentId
            };
        }

        public async Task DeleteJobTitleAsync(Guid id)
        {
            var jobTitle = await _repository.GetByIdAsync(id);
            if (jobTitle == null) throw new Exception("Job title not found");

            await _repository.DeleteAsync(jobTitle.Id);
        }
    }
}
