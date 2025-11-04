using HRMS.Application.DTOs.Companies;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CompanyDto?> GetByIdAsync(Guid id)
        {
            var company = await _unitOfWork.Company.GetByIdAsync(id);
            if (company == null) return null;

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

        public async Task<List<CompanyDto>> GetAllAsync()
        {
            var companies = await _unitOfWork.Company.GetAllAsync();
            return companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Address = c.Address,
                ContactEmail = c.ContactEmail,
                ContactPhone = c.ContactPhone
            }).ToList();
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

            await _unitOfWork.Company.AddAsync(company);
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

        public async Task<CompanyDto?> UpdateAsync(CompanyUpdateDto dto)
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            var company = await _unitOfWork.Company.GetByIdAsync(id);
            if (company == null) return false;

            await _unitOfWork.Company.DeleteAsync(company.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
