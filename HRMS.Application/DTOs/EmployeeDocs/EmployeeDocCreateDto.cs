using System;
using Microsoft.AspNetCore.Http;

namespace HRMS.Application.DTOs.EmployeeDocs
{
    public class EmployeeDocCreateDto
    {
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentDescription { get; set; } = string.Empty;
        public IFormFile Document { get; set; } = null!;
        public byte[]? File { get; set; }
        public Guid EmployeeId { get; set; }
    }
} 