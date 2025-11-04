using HRMS.Application.DTOs.EmployeeDocs;
using HRMS.Application.Interfaces.EmployeeDocs;
using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class EmployeeDocService : IEmployeeDocService
    {
        private readonly IEmployeeDocRepository _repository;
        private readonly IFileService _fileService;

        public EmployeeDocService(IEmployeeDocRepository repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<IEnumerable<EmployeeDocDto>> GetAllEmployeeDocsAsync()
        {
            var employeeDocs = await _repository.GetAllAsync();
            return employeeDocs.Select(ed => new EmployeeDocDto
            {
                Id = ed.Id,
                EmployeeId = ed.EmployeeId,
                DocumentType = ed.DocumentType,
                DocumentName = ed.DocumentName,
                FilePath = ed.FilePath,
                FileSize = ed.FileSize,
                UploadedDate = ed.UploadedDate,
                EmployeeName = ed.Employee?.GetFullName()
            });
        }

        public async Task<EmployeeDocDto?> GetEmployeeDocByIdAsync(Guid id)
        {
            var employeeDoc = await _repository.GetByIdAsync(id);
            if (employeeDoc == null) return null;

            return new EmployeeDocDto
            {
                Id = employeeDoc.Id,
                EmployeeId = employeeDoc.EmployeeId,
                DocumentType = employeeDoc.DocumentType,
                DocumentName = employeeDoc.DocumentName,
                FilePath = employeeDoc.FilePath,
                FileSize = employeeDoc.FileSize,
                UploadedDate = employeeDoc.UploadedDate,
                EmployeeName = employeeDoc.Employee?.GetFullName()
            };
        }

        public async Task<IEnumerable<EmployeeDocDto>> GetEmployeeDocsByEmployeeIdAsync(Guid employeeId)
        {
            var employeeDocs = await _repository.GetByEmployeeIdAsync(employeeId);
            return employeeDocs.Select(ed => new EmployeeDocDto
            {
                Id = ed.Id,
                EmployeeId = ed.EmployeeId,
                DocumentType = ed.DocumentType,
                DocumentName = ed.DocumentName,
                FilePath = ed.FilePath,
                FileSize = ed.FileSize,
                UploadedDate = ed.UploadedDate,
                EmployeeName = ed.Employee?.GetFullName()
            });
        }

        public async Task<EmployeeDocDto> CreateEmployeeDocAsync(EmployeeDocCreateDto dto)
        {
            var filePath = await _fileService.SaveFileAsync(dto.Document, "EmployeeDocs");
            
            var employeeDoc = new EmployeeDocs
            {
                Id = Guid.NewGuid(),
                EmployeeId = dto.EmployeeId,
                DocumentType = dto.DocumentType,
                DocumentName = dto.DocumentName,
                FilePath = filePath,
                FileSize = dto.File.Length,
                UploadedDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(employeeDoc);

            return new EmployeeDocDto
            {
                Id = employeeDoc.Id,
                EmployeeId = employeeDoc.EmployeeId,
                DocumentType = employeeDoc.DocumentType,
                DocumentName = employeeDoc.DocumentName,
                FilePath = employeeDoc.FilePath,
                FileSize = employeeDoc.FileSize,
                UploadedDate = employeeDoc.UploadedDate
            };
        }

        public async Task<EmployeeDocDto?> UpdateEmployeeDocAsync(EmployeeDocUpdateDto dto)
        {
            var employeeDoc = await _repository.GetByIdAsync(dto.Id);
            if (employeeDoc == null) return null;

            employeeDoc.DocumentType = dto.DocumentType;
            employeeDoc.DocumentName = dto.DocumentName;
            employeeDoc.UpdatedAt = DateTime.UtcNow;

            if (dto.Document != null)
            {
                // Delete old file
                await _fileService.DeleteFileAsync(employeeDoc.FilePath);
                
                // Save new file
                var newFilePath = await _fileService.SaveFileAsync(dto.Document, "EmployeeDocs");
                employeeDoc.FilePath = newFilePath;
                employeeDoc.FileSize = dto.Document.Length;
            }

            await _repository.UpdateAsync(employeeDoc);

            return new EmployeeDocDto
            {
                Id = employeeDoc.Id,
                EmployeeId = employeeDoc.EmployeeId,
                DocumentType = employeeDoc.DocumentType,
                DocumentName = employeeDoc.DocumentName,
                FilePath = employeeDoc.FilePath,
                FileSize = employeeDoc.FileSize,
                UploadedDate = employeeDoc.UploadedDate
            };
        }

        public async Task DeleteEmployeeDocAsync(Guid id)
        {
            var employeeDoc = await _repository.GetByIdAsync(id);
            if (employeeDoc == null) throw new Exception("Employee document not found");

            // Delete file from storage
            await _fileService.DeleteFileAsync(employeeDoc.FilePath);

            await _repository.DeleteAsync(employeeDoc.Id);
        }
    }
}
