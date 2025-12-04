using HRMS.Application.Interfaces.Companies;
using HRMS.Application.Interfaces.Departments;
using HRMS.Application.Interfaces.EmployeeDocs;
using HRMS.Application.Interfaces.Employees;
using HRMS.Application.Interfaces.JobTitles;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IDepartmentRepository Department { get; }
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        IJobTitleRepository JobTitle { get; }
        IEmployeeDocRepository EmployeeDoc { get; }
        IGeneric<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
} 