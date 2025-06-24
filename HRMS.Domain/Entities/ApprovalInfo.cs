using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class ApprovalInfo
    {
        [Required]
        public string ApprovedBy { get; set; } = string.Empty;

        [Required]
        public DateTime ApprovedDate { get; set; }

        public string? Comments { get; set; }
    }
}
