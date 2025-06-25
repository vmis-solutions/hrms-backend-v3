using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;

namespace HRMS.Application.Departments
{
    // TODO: This repository should eventually inherit from IGeneric<Company>
    public interface IDepartmentRepository : IGeneric<Department>
    {
        // Add company-specific repository methods here if needed
    }
} 