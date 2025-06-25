using HRMS.Application.DTOs;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Companies;
using HRMS.Application.Interfaces.Departments;
using HRMS.Application.NewFolder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Application.UseCases.Companies
{
    public class GetDepartmentUseCase : IGetAllDepartmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetDepartmentUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<DepartmentDto>> ExecuteAsync()
        {
            var companies = await _unitOfWork.Department.GetAllAsync();
            return companies.Select(company => new DepartmentDto
            {
               
            }).ToList();
        }


    }
} 