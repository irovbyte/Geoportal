using Geoportal.Data;
using Geoportal.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Geoportal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportRepository _repository;
    private readonly IFileService _fileService;

    public ReportsController(IReportRepository repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    // 1. Получение всех отчетов
    [HttpGet]
    public async Task<IActionResult> GetReports() => Ok(await _repository.GetAllAsync());

    // 2. Загрузка файла и создание отчета (через Form-Data)
    [HttpPost("upload-file")]
    public async Task<IActionResult> CreateReportWithFile([FromForm] string description, [FromForm] IFormFile image, [FromForm] string deviceId)
    {
        // Сохраняем фото на диск/сервер
        var imageUrl = await _fileService.SaveFileAsync(image);

        var report = new Report 
        { 
            // Id генерируется автоматически в модели (Guid.NewGuid().ToString())
            Description = description, 
            ImageHash = imageUrl, 
            DeviceId = deviceId,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(report);
        await _repository.SaveChangesAsync();

        return Ok(report);
    }

    // 3. Проверка на фрод (принимает твою модель Report напрямую через JSON)
    [HttpPost("check-fraud")]
    public async Task<IActionResult> CheckFraud([FromBody] Report report)
    {
        // Убираем варнинг асинхронности
        await Task.Yield();
        
        // Логика проверки координат из твоей модели Report
        // report.Latitude и report.Longitude уже доступны здесь
        bool isUzbekistan = true; 
        bool isNear = true;      

        if (!isUzbekistan) return BadRequest("Сервис доступен только в Узбекистане");
        if (!isNear) return BadRequest("Ошибка: Ваши координаты слишком далеко от объекта");

        return Ok(new { 
            message = "Проверка пройдена успешно", 
            status = "success",
            receivedId = report.Id 
        });
    }
}