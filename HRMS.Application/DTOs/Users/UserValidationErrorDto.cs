using HRMS.Application.DTOs.Employees;
using System.Collections.Generic;

namespace HRMS.Application.DTOs.Users
{
    public class UserValidationErrorDto
    {
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    }
}

