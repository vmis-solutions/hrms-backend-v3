using HRMS.Application.DTOs;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.Interfaces.Departments;
using HRMS.Application.NewFolder;
using HRMS.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Department
{
    public class UpdateDepartmentUseCase : IUpdateDepartmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateDepartmentUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DepartmentDto?> ExecuteAsync(DepartmentUpdateDto dto)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(Guid.NewGuid());
            if (department == null) return null;
            
            await _unitOfWork.Department.UpdateAsync(department);
            await _unitOfWork.SaveChangesAsync();
            return new DepartmentDto
            {
                
            };
        }

        
    }
} 