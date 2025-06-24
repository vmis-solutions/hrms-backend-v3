using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICompanyRepository Companies { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Companies = new CompanyRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 