using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities
{
    public class LeaveDay
    {
        public DateTime Date { get; set; }
        public bool IsPaid { get; set; }
    }
}
