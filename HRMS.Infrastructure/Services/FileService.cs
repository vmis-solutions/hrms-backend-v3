using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HRMS.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _webRootPath;

        public FileService()
        {
            _webRootPath = "wwwroot/EmployeeDocs";
        }

        public async Task<string> SaveFileAsync(IFormFile  file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null");

            // Create folder if it doesn't exist
            var uploadPath = Path.Combine(_webRootPath, folderName);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for database storage
            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var fullPath = Path.Combine(_webRootPath, filePath);
            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public bool FileExists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var fullPath = Path.Combine(_webRootPath, filePath);
            return File.Exists(fullPath);
        }
    }
} 