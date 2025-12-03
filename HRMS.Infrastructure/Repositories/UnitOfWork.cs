using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.Interfaces.Departments;
using HRMS.Application.Interfaces.Employees;
using HRMS.Application.Interfaces.JobTitles;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using HRMS.Infrastructure.Persistence.Data;
using System;
using HRMS.Application.Interfaces.EmployeeDocs;

namespace HRMS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IDepartmentRepository Department { get; }
        public ICompanyRepository Company { get; }
        public IEmployeeRepository Employee { get; }
        public IJobTitleRepository JobTitle { get; }

        public IEmployeeDocRepository EmployeeDoc { get; }

        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Company = new CompanyRepository(context);
            Department = new DepartmentRepository(context);
            Employee = new EmployeeRepository(context);
            JobTitle = new JobTitleRepository(context);
            EmployeeDoc = new EmployeeDocRepository(context);
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