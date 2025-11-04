using HRMS.Application.DTOs.Departments;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Departments;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DepartmentDto?> GetByIdAsync(Guid id)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(id);
            if (department == null) return null;

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CompanyId = department.CompanyId,
                CompanyName = department.Company?.Name
            };
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            var departments = await _unitOfWork.Department.GetAllAsync();
            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CompanyId = d.CompanyId,
                CompanyName = d.Company?.Name
            }).ToList();
        }

        public async Task<DepartmentDto> CreateAsync(DepartmentCreateDto dto)
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CompanyId = dto.CompanyId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Department.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CompanyId = department.CompanyId
            };
        }

        public async Task<DepartmentDto?> UpdateAsync(DepartmentUpdateDto dto)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(dto.Id);
            if (department == null) return null;

            department.Name = dto.Name;
            department.Description = dto.Description;
            department.CompanyId = dto.CompanyId;
            department.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Department.UpdateAsync(department);
            await _unitOfWork.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CompanyId = department.CompanyId
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var department = await _unitOfWork.Department.GetByIdAsync(id);
            if (department == null) return false;

            await _unitOfWork.Department.DeleteAsync(department.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
