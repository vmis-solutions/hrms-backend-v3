using HRMS.Application.Departments;
using HRMS.Application.Interfaces.Companies;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IDepartmentRepository Department { get; }
        ICompanyRepository Company { get; }
        IGeneric<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
} 