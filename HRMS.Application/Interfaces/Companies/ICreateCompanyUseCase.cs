using HRMS.Application.DTOs;
using HRMS.Application.NewFolder;

namespace HRMS.Application.Interfaces.Companies
{
    public interface ICreateCompanyUseCase
    {
        Task<CompanyDto> ExecuteAsync(CompanyCreateDto dto);
    }
} 