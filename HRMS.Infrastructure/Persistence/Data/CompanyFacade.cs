using HRMS.Application.Interfaces;
using HRMS.Application.NewFolder;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Persistence.Data
{
    public class CompanyFacade : ICompanyFacade
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyFacade(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CompanyDto?> GetByIdAsync(Guid id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);
            return company == null ? null : ToDto(company);
        }

        public async Task<List<CompanyDto>> GetAllAsync()
        {
            var companies = await _unitOfWork.Companies.GetAllAsync();
            return companies.Select(ToDto).ToList();
        }

        public async Task<CompanyDto> CreateAsync(CompanyCreateDto dto)
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();
            return ToDto(company);
        }

        public async Task<CompanyDto?> UpdateAsync(CompanyUpdateDto dto)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(dto.Id);
            if (company == null) return null;
            company.Name = dto.Name;
            company.Description = dto.Description;
            company.Address = dto.Address;
            company.ContactEmail = dto.ContactEmail;
            company.ContactPhone = dto.ContactPhone;
            company.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Companies.UpdateAsync(company);
            await _unitOfWork.SaveChangesAsync();
            return ToDto(company);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);
            if (company == null) return false;
            await _unitOfWork.Companies.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static CompanyDto ToDto(Company company)
        {
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