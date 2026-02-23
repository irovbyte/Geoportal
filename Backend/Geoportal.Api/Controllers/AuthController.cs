using Geoportal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Geoportal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.PhoneNumber == dto.PhoneNumber))
            return BadRequest("Этот номер уже зарегистрирован");

        // Если у тебя User.Id это строка (string)
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = dto.Password,
            DeviceId = dto.DeviceId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Регистрация успешна!", userId = user.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber && u.PasswordHash == dto.Password);

        if (user == null) return Unauthorized("Неверный номер или пароль");

        return Ok(new { message = "Вход выполнен", userId = user.Id });
    }
}

// DTO классы
public record UserRegistrationDto(string PhoneNumber, string Password, string DeviceId);
public record UserLoginDto(string PhoneNumber, string Password);