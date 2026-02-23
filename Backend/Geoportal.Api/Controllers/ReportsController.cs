using Geoportal.Data;
using Geoportal.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Geoportal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportRepository _repository;

    public ReportsController(IReportRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetReports() => Ok(await _repository.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] Report report)
    {
        report.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(report);
        await _repository.SaveChangesAsync();
        return Ok(report);
    }
    [HttpPost("upload")]
    public async Task<IActionResult> CreateReportWithFile([FromForm] string description, [FromForm] IFormFile image)
    {
        // 1. Сохраняем фото через сервис
        var imageUrl = await _fileService.SaveFileAsync(image);

        // 2. Создаем запись в базе
        var report = new Report 
        { 
            Description = description, 
            ImageUrl = imageUrl,
            Status = "Pending" // Отправляем на проверку AI
        };

        await _repository.AddAsync(report);
        await _repository.SaveChangesAsync();

        return Ok(report);
    }
    [HttpPost("upload")]
    public async Task<IActionResult> CreateReport([FromForm] ReportRequest request)
    {
        // 1. Проверка IP (Узбекистан)
        var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (!IsUzbekistanIp(userIp)) 
        {
            return BadRequest("Регистрация разрешена только с территории Узбекистана.");
        }

        // 2. Проверка расстояния (чтобы человек был рядом с объектом)
        // Если GPS телефона далеко от координат ямы — это фрод
        if (!IsUserNearObject(request.UserLat, request.UserLng, request.ObjectLat, request.ObjectLng))
        {
            return BadRequest("Вы должны находиться рядом с объектом для отправки отчета.");
        }
    }
}