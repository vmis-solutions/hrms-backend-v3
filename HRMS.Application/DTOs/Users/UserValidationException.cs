using System;

namespace HRMS.Application.DTOs.Users
{
    public class UserValidationException : InvalidOperationException
    {
        public UserValidationErrorDto ValidationErrors { get; }

        public UserValidationException(UserValidationErrorDto validationErrors)
            : base("User validation failed")
        {
            ValidationErrors = validationErrors;
        }
    }
}

