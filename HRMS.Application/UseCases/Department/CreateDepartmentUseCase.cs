using HRMS.Application.DTOs;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Departments;
using md = HRMS.Domain.Entities;

namespace HRMS.Application.UseCases.Department
{
    public class CreateDepartmentUseCase : ICreateDepartmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateDepartmentUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DepartmentDto> ExecuteAsync(DepartmentCreateDto dto)
        {
            var department = new md.Department
            {
               
            };
            await _unitOfWork.Department.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();
            return new DepartmentDto
            {
               
            };

        }


    }
} 