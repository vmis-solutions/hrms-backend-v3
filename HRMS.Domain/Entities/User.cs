using System;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Domain.Entities
{
    public class User : IdentityUser
    {
        public Guid? EmployeeId { get; set; }
        //  public Employee? Employee { get; set; }
    }
}
