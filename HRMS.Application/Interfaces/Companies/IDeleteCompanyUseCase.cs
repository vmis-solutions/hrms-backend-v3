using System;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Companies
{
    public interface IDeleteCompanyUseCase
    {
        Task<bool> ExecuteAsync(Guid id);
    }
} 