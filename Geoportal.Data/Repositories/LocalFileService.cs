using Microsoft.AspNetCore.Http;
using Geoportal.Data.Interfaces;

namespace Geoportal.Data.Repositories;

public class LocalFileService : IFileService
{
    private readonly string _storagePath;
    private readonly string _baseUrl = "http://136.113.150.143:5001"; // Твой IP

    public LocalFileService()
    {
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_storagePath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"{_baseUrl}/uploads/{fileName}";
    }
}