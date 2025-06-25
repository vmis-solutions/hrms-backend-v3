using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.Interfaces.Departments;
using System;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class DeleteDepartmentUseCase : IDeleteDepartmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteDepartmentUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> ExecuteAsync(Guid id)
        {
            var company = await _unitOfWork.Department.GetByIdAsync(id);
            if (company == null) return false;
            await _unitOfWork.Department.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
} 