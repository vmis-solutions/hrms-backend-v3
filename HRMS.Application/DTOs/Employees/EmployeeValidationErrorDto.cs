using System.Collections.Generic;

namespace HRMS.Application.DTOs.Employees
{
    public class EmployeeValidationErrorDto
    {
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    }

    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}

