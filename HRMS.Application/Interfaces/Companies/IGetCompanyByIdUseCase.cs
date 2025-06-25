using HRMS.Application.NewFolder;
using System;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Companies
{
    public interface IGetCompanyByIdUseCase
    {
        Task<CompanyDto?> ExecuteAsync(Guid id);
    }
} 