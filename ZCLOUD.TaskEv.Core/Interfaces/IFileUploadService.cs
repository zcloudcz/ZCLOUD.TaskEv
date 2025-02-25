
using Microsoft.AspNetCore.Http;

namespace ZCLOUD.TaskEv.Core.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadFile(IFormFile file, int taskId);
    Task DeleteFile(string filePath);
}

