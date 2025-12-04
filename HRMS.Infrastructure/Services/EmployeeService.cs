using HRMS.Application.Common;
using HRMS.Application.DTOs.Employees;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Employees;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _unitOfWork.Employee.GetByIdAsync(id);
            if (employee == null) return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Email = employee.Email,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                CivilStatus = employee.CivilStatus,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                SssNumber = employee.SssNumber,
                PhilHealthNumber = employee.PhilHealthNumber,
                PagIbigNumber = employee.PagIbigNumber,
                Tin = employee.Tin,
                EmployeeNumber = employee.EmployeeNumber,
                DateHired = employee.DateHired,
                CompanyId = employee.CompanyId,
                DepartmentId = employee.DepartmentId,
                JobTitleId = employee.JobTitleId,
                EmploymentStatus = employee.EmploymentStatus,
                Avatar = employee.Avatar,
                CompanyName = employee.Company?.Name,
                DepartmentName = employee.Department?.Name,
                JobTitleName = employee.JobTitle?.Title
            };
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.Employee.GetAllAsync();
            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                UserId = e.UserId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Email = e.Email,
                BirthDate = e.BirthDate,
                Gender = e.Gender,
                CivilStatus = e.CivilStatus,
                PhoneNumber = e.PhoneNumber,
                Address = e.Address,
                SssNumber = e.SssNumber,
                PhilHealthNumber = e.PhilHealthNumber,
                PagIbigNumber = e.PagIbigNumber,
                Tin = e.Tin,
                EmployeeNumber = e.EmployeeNumber,
                DateHired = e.DateHired,
                CompanyId = e.CompanyId,
                DepartmentId = e.DepartmentId,
                JobTitleId = e.JobTitleId,
                EmploymentStatus = e.EmploymentStatus,
                Avatar = e.Avatar,
                CompanyName = e.Company?.Name,
                DepartmentName = e.Department?.Name,
                JobTitleName = e.JobTitle?.Title
            });
        }

        public async Task<PagedResult<EmployeeDto>> GetEmployeesByUserRoleAsync(Guid userId, string role, int pageNumber, int pageSize, string? searchTerm = null)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            IEnumerable<Employee> employees;
            int totalCount;

            if (role?.ToUpper() == "HR_MANAGER")
            {
                (employees, totalCount) = await _unitOfWork.Employee.GetAllPaginatedAsync(pageNumber, pageSize, searchTerm);
            }
            else
            {
                var currentEmployee = await _unitOfWork.Employee.GetByIdAsync(userId);
                if (currentEmployee == null)
                {
                    return PagedResult<EmployeeDto>.Empty(pageNumber, pageSize);
                }

                (employees, totalCount) = await _unitOfWork.Employee.GetEmployeesByHrManagerIdPaginatedAsync(currentEmployee.Id, pageNumber, pageSize, searchTerm);
            }

            return new PagedResult<EmployeeDto>
            {
                Items = employees.Select(MapEmployee),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto dto)
        {
            var validationErrors = new EmployeeValidationErrorDto();

            // Check for duplicate email
            var existingEmployeeByEmail = await _unitOfWork.Employee.GetByEmailAsync(dto.Email);
            if (existingEmployeeByEmail != null)
            {
                validationErrors.Errors.Add(new ValidationError
                {
                    Field = nameof(dto.Email),
                    Message = $"An employee with email '{dto.Email}' already exists.",
                    Value = dto.Email
                });
            }

            // Check for duplicate employee number
            var existingEmployeeByNumber = await _unitOfWork.Employee.GetByEmployeeNumberAsync(dto.EmployeeNumber);
            if (existingEmployeeByNumber != null)
            {
                validationErrors.Errors.Add(new ValidationError
                {
                    Field = nameof(dto.EmployeeNumber),
                    Message = $"An employee with employee number '{dto.EmployeeNumber}' already exists.",
                    Value = dto.EmployeeNumber
                });
            }

            // Check for duplicate government IDs (only if provided)
            if (!string.IsNullOrWhiteSpace(dto.SssNumber))
            {
                var existingEmployeeBySss = await _unitOfWork.Employee.GetBySssNumberAsync(dto.SssNumber);
                if (existingEmployeeBySss != null)
                {
                    validationErrors.Errors.Add(new ValidationError
                    {
                        Field = nameof(dto.SssNumber),
                        Message = $"An employee with SSS number '{dto.SssNumber}' already exists.",
                        Value = dto.SssNumber
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.PhilHealthNumber))
            {
                var existingEmployeeByPhilHealth = await _unitOfWork.Employee.GetByPhilHealthNumberAsync(dto.PhilHealthNumber);
                if (existingEmployeeByPhilHealth != null)
                {
                    validationErrors.Errors.Add(new ValidationError
                    {
                        Field = nameof(dto.PhilHealthNumber),
                        Message = $"An employee with PhilHealth number '{dto.PhilHealthNumber}' already exists.",
                        Value = dto.PhilHealthNumber
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.PagIbigNumber))
            {
                var existingEmployeeByPagIbig = await _unitOfWork.Employee.GetByPagIbigNumberAsync(dto.PagIbigNumber);
                if (existingEmployeeByPagIbig != null)
                {
                    validationErrors.Errors.Add(new ValidationError
                    {
                        Field = nameof(dto.PagIbigNumber),
                        Message = $"An employee with Pag-Ibig number '{dto.PagIbigNumber}' already exists.",
                        Value = dto.PagIbigNumber
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.Tin))
            {
                var existingEmployeeByTin = await _unitOfWork.Employee.GetByTinAsync(dto.Tin);
                if (existingEmployeeByTin != null)
                {
                    validationErrors.Errors.Add(new ValidationError
                    {
                        Field = nameof(dto.Tin),
                        Message = $"An employee with TIN '{dto.Tin}' already exists.",
                        Value = dto.Tin
                    });
                }
            }

            // Throw exception with all validation errors if any exist
            if (validationErrors.Errors.Count > 0)
            {
                throw new EmployeeValidationException(validationErrors);
            }

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                Email = dto.Email,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                CivilStatus = dto.CivilStatus,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                SssNumber = dto.SssNumber,
                PhilHealthNumber = dto.PhilHealthNumber,
                PagIbigNumber = dto.PagIbigNumber,
                Tin = dto.Tin,
                EmployeeNumber = dto.EmployeeNumber,
                DateHired = dto.DateHired,
                CompanyId = dto.CompanyId,
                DepartmentId = dto.DepartmentId,
                JobTitleId = dto.JobTitleId,
                EmploymentStatus = dto.EmploymentStatus,
                Avatar = dto.Avatar,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Employee.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            return new EmployeeDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Email = employee.Email,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                CivilStatus = employee.CivilStatus,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                SssNumber = employee.SssNumber,
                PhilHealthNumber = employee.PhilHealthNumber,
                PagIbigNumber = employee.PagIbigNumber,
                Tin = employee.Tin,
                EmployeeNumber = employee.EmployeeNumber,
                DateHired = employee.DateHired,
                CompanyId = employee.CompanyId,
                DepartmentId = employee.DepartmentId,
                JobTitleId = employee.JobTitleId,
                EmploymentStatus = employee.EmploymentStatus,
                Avatar = employee.Avatar
            };
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(EmployeeUpdateDto dto)
        {
            var employee = await _unitOfWork.Employee.GetByIdAsync(dto.Id);
            if (employee == null) throw new Exception("Employee not found");

            // Check for duplicate email (if email is being changed)
            if (employee.Email != dto.Email)
            {
                var existingEmployeeByEmail = await _unitOfWork.Employee.GetByEmailAsync(dto.Email);
                if (existingEmployeeByEmail != null && existingEmployeeByEmail.Id != dto.Id)
                {
                    throw new InvalidOperationException($"An employee with email '{dto.Email}' already exists.");
                }
            }

            // Check for duplicate employee number (if employee number is being changed)
            if (employee.EmployeeNumber != dto.EmployeeNumber)
            {
                var existingEmployeeByNumber = await _unitOfWork.Employee.GetByEmployeeNumberAsync(dto.EmployeeNumber);
                if (existingEmployeeByNumber != null && existingEmployeeByNumber.Id != dto.Id)
                {
                    throw new InvalidOperationException($"An employee with employee number '{dto.EmployeeNumber}' already exists.");
                }
            }

            // Check for duplicate government IDs (only if provided and being changed)
            if (!string.IsNullOrWhiteSpace(dto.SssNumber) && employee.SssNumber != dto.SssNumber)
            {
                var existingEmployeeBySss = await _unitOfWork.Employee.GetBySssNumberAsync(dto.SssNumber);
                if (existingEmployeeBySss != null && existingEmployeeBySss.Id != dto.Id)
                {
                    throw new InvalidOperationException($"An employee with SSS number '{dto.SssNumber}' already exists.");
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.PhilHealthNumber) && employee.PhilHealthNumber != dto.PhilHealthNumber)
            {
                var existingEmployeeByPhilHealth = await _unitOfWork.Employee.GetByPhilHealthNumberAsync(dto.PhilHealthNumber);
                if (existingEmployeeByPhilHealth != null && existingEmployeeByPhilHealth.Id != dto.Id)
                {
                    throw new InvalidOperationException($"An employee with PhilHealth number '{dto.PhilHealthNumber}' already exists.");
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.PagIbigNumber) && employee.PagIbigNumber != dto.PagIbigNumber)
            {
                var existingEmployeeByPagIbig = await _unitOfWork.Employee.GetByPagIbigNumberAsync(dto.PagIbigNumber);
                if (existingEmployeeByPagIbig != null && existingEmployeeByPagIbig.Id != dto.Id)
                {
                    throw new InvalidOperationException($"An employee with Pag-Ibig number '{dto.PagIbigNumber}' already exists.");
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.Tin) && employee.Tin != dto.Tin)
            {
                var existingEmployeeByTin = await _unitOfWork.Employee.GetByTinAsync(dto.Tin);
                if (existingEmployeeByTin != null && existingEmployeeByTin.Id != dto.Id)
                {
                    throw new InvalidOperationException($"An employee with TIN '{dto.Tin}' already exists.");
                }
            }

            employee.UserId = dto.UserId;
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.MiddleName = dto.MiddleName;
            employee.Email = dto.Email;
            employee.BirthDate = dto.BirthDate;
            employee.Gender = dto.Gender;
            employee.CivilStatus = dto.CivilStatus;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.Address = dto.Address;
            employee.SssNumber = dto.SssNumber;
            employee.PhilHealthNumber = dto.PhilHealthNumber;
            employee.PagIbigNumber = dto.PagIbigNumber;
            employee.Tin = dto.Tin;
            employee.EmployeeNumber = dto.EmployeeNumber;
            employee.DateHired = dto.DateHired;
            employee.CompanyId = dto.CompanyId;
            employee.DepartmentId = dto.DepartmentId;
            employee.JobTitleId = dto.JobTitleId;
            employee.EmploymentStatus = dto.EmploymentStatus;
            employee.Avatar = dto.Avatar;
            employee.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Employee.UpdateAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            return new EmployeeDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Email = employee.Email,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                CivilStatus = employee.CivilStatus,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                SssNumber = employee.SssNumber,
                PhilHealthNumber = employee.PhilHealthNumber,
                PagIbigNumber = employee.PagIbigNumber,
                Tin = employee.Tin,
                EmployeeNumber = employee.EmployeeNumber,
                DateHired = employee.DateHired,
                CompanyId = employee.CompanyId,
                DepartmentId = employee.DepartmentId,
                JobTitleId = employee.JobTitleId,
                EmploymentStatus = employee.EmploymentStatus,
                Avatar = employee.Avatar
            };
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await _unitOfWork.Employee.GetByIdAsync(id);
            if (employee == null) throw new Exception("Employee not found");

            await _unitOfWork.Employee.DeleteAsync(employee.Id);
            await _unitOfWork.SaveChangesAsync();
        }

        private static EmployeeDto MapEmployee(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Email = employee.Email,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                CivilStatus = employee.CivilStatus,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                SssNumber = employee.SssNumber,
                PhilHealthNumber = employee.PhilHealthNumber,
                PagIbigNumber = employee.PagIbigNumber,
                Tin = employee.Tin,
                EmployeeNumber = employee.EmployeeNumber,
                DateHired = employee.DateHired,
                CompanyId = employee.CompanyId,
                DepartmentId = employee.DepartmentId,
                JobTitleId = employee.JobTitleId,
                EmploymentStatus = employee.EmploymentStatus,
                Avatar = employee.Avatar,
                CompanyName = employee.Company?.Name,
                DepartmentName = employee.Department?.Name,
                JobTitleName = employee.JobTitle?.Title
            };
        }
    }
}
