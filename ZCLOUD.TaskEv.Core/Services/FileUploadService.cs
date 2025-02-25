
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ZCLOUD.TaskEv.Core.Interfaces;

namespace ZCLOUD.TaskEv.Core.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileUploadService> _logger;

    public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async Task<string> UploadFile(IFormFile file, int taskId)
    {
        try
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", taskId.ToString());
            Directory.CreateDirectory(uploadPath);

            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chyba při nahrávání souboru");
            throw;
        }
    }

    public async Task DeleteFile(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_environment.WebRootPath, "uploads", filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chyba při mazání souboru");
            throw;
        }
    }
}


