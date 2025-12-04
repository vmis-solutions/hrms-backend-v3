using Microsoft.AspNetCore.Http;

namespace HRMS.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFileAsync(string filePath);
        bool FileExists(string filePath);
    }
} 