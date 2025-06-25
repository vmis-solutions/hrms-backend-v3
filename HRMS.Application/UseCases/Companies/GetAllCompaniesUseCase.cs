using HRMS.Application.Interfaces.Companies;
using HRMS.Application.NewFolder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class GetAllCompaniesUseCase : IGetAllCompaniesUseCase
    {
        private readonly ICompanyRepository _companyRepository;
        public GetAllCompaniesUseCase(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<List<CompanyDto>> ExecuteAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.Select(company => new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                Address = company.Address,
                ContactEmail = company.ContactEmail,
                ContactPhone = company.ContactPhone
            }).ToList();
        }
    }
} 