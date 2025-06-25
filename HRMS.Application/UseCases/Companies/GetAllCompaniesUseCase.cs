using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.NewFolder;


namespace HRMS.Application.UseCases.Companies
{
    public class GetCompanyUseCase : IGetAllCompanyUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCompanyUseCase(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public async Task<List<CompanyDto>> ExecuteAsync()
        {
            var companies = await _unitOfWork.Company.GetAllAsync();
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