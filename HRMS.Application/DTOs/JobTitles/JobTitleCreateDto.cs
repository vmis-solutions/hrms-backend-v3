using System;

namespace HRMS.Application.DTOs.JobTitles
{
    public class JobTitleCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid DepartmentId { get; set; }
    }
} 