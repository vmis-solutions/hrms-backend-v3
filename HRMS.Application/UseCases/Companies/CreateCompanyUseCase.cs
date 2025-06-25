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
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateCompanyUseCase(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
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
            await _companyRepository.AddAsync(company);
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