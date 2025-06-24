using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using System;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class DeleteCompanyUseCase : IDeleteCompanyUseCase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCompanyUseCase(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> ExecuteAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return false;
            await _companyRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
} 