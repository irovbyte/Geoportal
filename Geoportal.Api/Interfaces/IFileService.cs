namespace Geoportal.Api.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
}