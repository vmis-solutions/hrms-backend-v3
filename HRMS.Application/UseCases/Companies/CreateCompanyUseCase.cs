using HRMS.Application.DTOs;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.NewFolder;
using HRMS.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class CreateCompanyUseCase : ICreateCompanyUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCompanyUseCase( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CompanyDto> ExecuteAsync(CompanyCreateDto dto)
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

        public Task<DepartmentDto> ExecuteAsync(DepartmentCreateDto dto)
        {
            throw new NotImplementedException();
        }
    }
} 