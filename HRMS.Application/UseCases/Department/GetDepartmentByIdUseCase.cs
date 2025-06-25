using HRMS.Application.DTOs;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Departments;


namespace HRMS.Application.UseCases.Department
{
    public class GetDepartmentByIdUseCase : IGetDepartmentByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetDepartmentByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DepartmentDto?> ExecuteAsync(Guid id)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(id);
            if (department == null) return null;
            return new DepartmentDto
            {
          
            };
        }
    }
} 