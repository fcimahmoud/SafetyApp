
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;

namespace Services
{
    public class FileService (IWebHostEnvironment _env)
        : IFileService
    {

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, folderName);
            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine(folderName, fileName).Replace("\\", "/"); // Return relative path
        }

        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
