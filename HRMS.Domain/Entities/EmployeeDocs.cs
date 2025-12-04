using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class EmployeeDocs : BaseAuditable
    {
        [Required]
        public string DocumentName { get; set; } = string.Empty;

        [Required]
        public string DocumentType { get; set; } = string.Empty;

        [Required]
        public string DocumentDescription { get; set; } = string.Empty;

        [Required]
        public string DocumentPath { get; set; } = string.Empty;

        [Required]
        public Guid EmployeeId { get; set; }

        // File content for uploads
        public byte[]? File { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadedDate { get; set; }

        // Navigation property
        public Employee Employee { get; set; } = null!;
    }
}
