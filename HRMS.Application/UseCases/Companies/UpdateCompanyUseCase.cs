using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.NewFolder;
using HRMS.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class UpdateCompanyUseCase : IUpdateCompanyUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCompanyUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CompanyDto?> ExecuteAsync(CompanyUpdateDto dto)
        {
            var company = await _unitOfWork.Company.GetByIdAsync(dto.Id);
            if (company == null) return null;
            company.Name = dto.Name;
            company.Description = dto.Description;
            company.Address = dto.Address;
            company.ContactEmail = dto.ContactEmail;
            company.ContactPhone = dto.ContactPhone;
            company.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Company.UpdateAsync(company);
            await _unitOfWork.SaveChangesAsync();
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                Address = company.Address,
                ContactEmail = company.ContactEmail,
                ContactPhone = company.ContactPhone
            };
        }
    }
} 