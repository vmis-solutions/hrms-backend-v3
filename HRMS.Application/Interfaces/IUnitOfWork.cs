using System.Threading.Tasks;

namespace HRMS.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICompanyRepository Companies { get; }
        Task<int> SaveChangesAsync();
    }
} 