using System;

namespace HRMS.Application.DTOs.EmployeeDocs
{
    public class EmployeeDocDto
    {
        public Guid Id { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentDescription { get; set; } = string.Empty;
        public string DocumentPath { get; set; } = string.Empty;
        public Guid EmployeeId { get; set; }
        public byte[]? File { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadedDate { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 