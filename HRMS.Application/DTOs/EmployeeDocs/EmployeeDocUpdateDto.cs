using System;
using Microsoft.AspNetCore.Http;

namespace HRMS.Application.DTOs.EmployeeDocs
{
    public class EmployeeDocUpdateDto
    {
        public Guid Id { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentDescription { get; set; } = string.Empty;
        public IFormFile? Document { get; set; }
        public byte[]? File { get; set; }
        public Guid EmployeeId { get; set; }
    }
} 