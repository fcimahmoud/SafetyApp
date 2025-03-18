
global using Microsoft.AspNetCore.Http;

namespace Services.Abstractions
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        void DeleteFile(string filePath);
    }
}
