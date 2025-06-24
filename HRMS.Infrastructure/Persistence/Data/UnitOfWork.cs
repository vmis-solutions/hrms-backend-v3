using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace HRMS.Infrastructure.Persistence.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICompanyRepository Companies { get; }
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Companies = new CompanyRepository(context);
        }

        public IGeneric<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.TryGetValue(type, out var repo))
            {
                var genericRepo = new GenericRepository<T>(_context);
                _repositories[type] = genericRepo;
                return genericRepo;
            }
            return (IGeneric<T>)repo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 