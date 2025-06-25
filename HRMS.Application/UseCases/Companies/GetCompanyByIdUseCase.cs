using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.NewFolder;
using System;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class GetCompanyByIdUseCase : IGetCompanyByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCompanyByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CompanyDto?> ExecuteAsync(Guid id)
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
    }
} 