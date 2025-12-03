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
            var department = await _unitOfWork.Department.GetByIdWithHrManagersAsync(id);
            if (department == null) return null;

            return MapDepartmentToDto(department);
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            var departments = await _unitOfWork.Department.GetAllAsync();
            var departmentList = departments.ToList();
            var departmentDtos = new List<DepartmentDto>();

            foreach (var d in departmentList)
            {
                var department = await _unitOfWork.Department.GetByIdWithHrManagersAsync(d.Id);
                if (department == null) continue;

                departmentDtos.Add(MapDepartmentToDto(department));
            }

            return departmentDtos;
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

        public async Task<List<DepartmentHrManagerDto>> GetHrManagersByDepartmentIdAsync(Guid departmentId)
        {
            var hrManagers = await _unitOfWork.Department.GetHrManagersByDepartmentIdAsync(departmentId);
            return hrManagers.Select(dhm => new DepartmentHrManagerDto
            {
                Id = dhm.Id,
                DepartmentId = dhm.DepartmentId,
                EmployeeId = dhm.EmployeeId,
                EmployeeName = $"{dhm.Employee.FirstName} {dhm.Employee.LastName}",
                EmployeeEmail = dhm.Employee.Email,
                EmployeeNumber = dhm.Employee.EmployeeNumber,
                AssignedAt = dhm.CreatedAt
            }).ToList();
        }

        public async Task<bool> AssignHrManagersAsync(AssignHrManagersDto dto)
        {
            // Verify department exists
            var department = await _unitOfWork.Department.GetByIdAsync(dto.DepartmentId);
            if (department == null) return false;

            // Deduplicate employee IDs to prevent processing the same employee multiple times
            var uniqueEmployeeIds = dto.EmployeeIds.Distinct().ToList();

            // Verify all employees exist
            var employeeRepo = _unitOfWork.GetRepository<Employee>();
            var processedCombinations = new HashSet<(Guid DepartmentId, Guid EmployeeId)>();

            foreach (var employeeId in uniqueEmployeeIds)
            {
                var employee = await employeeRepo.GetByIdAsync(employeeId);
                if (employee == null) return false;

                var combination = (dto.DepartmentId, employeeId);
                
                // Skip if we've already processed this combination in this transaction
                if (processedCombinations.Contains(combination))
                    continue;

                processedCombinations.Add(combination);

                // Check if assignment already exists (including soft-deleted records and entities in change tracker)
                var existingHrManager = await _unitOfWork.Department.GetHrManagerByDepartmentAndEmployeeAsync(dto.DepartmentId, employeeId);
                
                if (existingHrManager != null)
                {
                    // If record exists but is soft-deleted, restore it
                    if (existingHrManager.IsDeleted)
                    {
                        existingHrManager.IsDeleted = false;
                        existingHrManager.UpdatedAt = DateTime.UtcNow;
                        await _unitOfWork.Department.UpdateHrManagerAsync(existingHrManager);
                    }
                    // If record exists and is not deleted, skip (already assigned or being added)
                    // This handles both existing records and entities in "Added" state in change tracker
                }
                else
                {
                    // Create new assignment - AddHrManagerAsync will do final validation
                    var departmentHrManager = new DepartmentHrManager
                    {
                        Id = Guid.NewGuid(),
                        DepartmentId = dto.DepartmentId,
                        EmployeeId = employeeId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.Department.AddHrManagerAsync(departmentHrManager);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveHrManagerAsync(Guid hrManagerId)
        {
            var hrManager = await _unitOfWork.Department.GetHrManagerByIdAsync(hrManagerId);
            if (hrManager == null) return false;

            await _unitOfWork.Department.RemoveHrManagerAsync(hrManagerId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<DepartmentDto>> GetDepartmentsForCurrentUserAsync(string userId, bool isHrCompanyRole)
        {
            if (!isHrCompanyRole)
            {
                return await GetAllAsync();
            }

            var departments = await _unitOfWork.Department.GetDepartmentsManagedByUserAsync(userId);
            return departments.Select(MapDepartmentToDto).ToList();
        }

        private static DepartmentDto MapDepartmentToDto(Department department)
        {
            var hrManagers = department.HrManagers?
                .Where(dhm => !dhm.IsDeleted)
                .Select(dhm => new DepartmentHrManagerDto
                {
                    Id = dhm.Id,
                    DepartmentId = dhm.DepartmentId,
                    EmployeeId = dhm.EmployeeId,
                    EmployeeName = dhm.Employee != null ? $"{dhm.Employee.FirstName} {dhm.Employee.LastName}" : string.Empty,
                    EmployeeEmail = dhm.Employee?.Email ?? string.Empty,
                    EmployeeNumber = dhm.Employee?.EmployeeNumber ?? string.Empty,
                    AssignedAt = dhm.CreatedAt
                }).ToList() ?? new List<DepartmentHrManagerDto>();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CompanyId = department.CompanyId,
                CompanyName = department.Company?.Name ?? string.Empty,
                EmployeeCount = department.Employees?.Count(e => !e.IsDeleted) ?? 0,
                HrManagers = hrManagers
            };
        }
    }
}
