using HRMS.Application.Interfaces.Companies;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICompanyRepository Companies { get; }
        IGeneric<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
} 