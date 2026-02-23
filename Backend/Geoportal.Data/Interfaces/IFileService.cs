using Microsoft.AspNetCore.Http;

namespace Geoportal.Data.Interfaces;

public interface IFileService
{
    // Возвращает путь к сохраненному файлу
    Task<string> SaveFileAsync(IFormFile file);
}