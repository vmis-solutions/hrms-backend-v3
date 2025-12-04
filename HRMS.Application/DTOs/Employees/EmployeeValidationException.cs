using System;

namespace HRMS.Application.DTOs.Employees
{
    public class EmployeeValidationException : InvalidOperationException
    {
        public EmployeeValidationErrorDto ValidationErrors { get; }

        public EmployeeValidationException(EmployeeValidationErrorDto validationErrors)
            : base("Employee validation failed")
        {
            ValidationErrors = validationErrors;
        }
    }
}

