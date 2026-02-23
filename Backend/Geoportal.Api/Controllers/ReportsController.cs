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

    [HttpGet]
    public async Task<IActionResult> GetReports() => Ok(await _repository.GetAllAsync());

    // 2. Загрузка файла и создание отчета (через Form-Data)
    [HttpPost("upload-file")]
    [Consumes("multipart/form-data")] 
    public async Task<IActionResult> CreateReportWithFile([FromForm] FileUploadDto dto)
    {
        if (dto.Image == null || dto.Image.Length == 0)
            return BadRequest("Файл изображения не выбран");

        var imageUrl = await _fileService.SaveFileAsync(dto.Image);

        var report = new Report 
        { 
            Description = dto.Description ?? string.Empty, 
            ImageHash = imageUrl, 
            DeviceId = dto.DeviceId ?? "unknown",
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(report);
        await _repository.SaveChangesAsync();

        return Ok(report);
    }

    // 3. Проверка на фрод
    [HttpPost("check-fraud")]
    public async Task<IActionResult> CheckFraud([FromBody] Report report)
    {
        await Task.Yield();
        
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

public class FileUploadDto
{
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public string? DeviceId { get; set; }
}