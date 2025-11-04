using HRMS.Application.DTOs.Employees;
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
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _repository.GetByIdAsync(id);
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
            var employees = await _repository.GetAllAsync();
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

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto dto)
        {
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

            await _repository.AddAsync(employee);

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
            var employee = await _repository.GetByIdAsync(dto.Id);
            if (employee == null) throw new Exception("Employee not found");

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

            await _repository.UpdateAsync(employee);

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
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null) throw new Exception("Employee not found");

            await _repository.DeleteAsync(employee.Id);
        }
    }
}
