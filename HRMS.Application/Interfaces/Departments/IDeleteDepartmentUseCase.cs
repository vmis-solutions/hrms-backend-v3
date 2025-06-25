using System;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Departments
{
    public interface IDeleteDepartmentUseCase
    {
        Task<bool> ExecuteAsync(Guid id);
    }
} 