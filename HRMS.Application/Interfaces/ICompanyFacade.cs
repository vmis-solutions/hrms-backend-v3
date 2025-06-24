using HRMS.Application.NewFolder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces
{
    public interface ICompanyFacade
    {
        Task<CompanyDto?> GetByIdAsync(Guid id);
        Task<List<CompanyDto>> GetAllAsync();
        Task<CompanyDto> CreateAsync(CompanyCreateDto dto);
        Task<CompanyDto?> UpdateAsync(CompanyUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
} 