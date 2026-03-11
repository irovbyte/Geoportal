using Geoportal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

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

        if (await _context.Users.AnyAsync(u => u.DeviceId == dto.DeviceId))
            return BadRequest("С этого устройства уже создан аккаунт.");

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
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
            .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Неверный номер или пароль");

        if (user.DeviceId != dto.DeviceId)
        {
            user.DeviceId = dto.DeviceId;
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = "Вход выполнен", userId = user.Id });
    }

    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteAccount(string phoneNumber)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        if (user == null) return NotFound("Пользователь не найден");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Аккаунт {phoneNumber} успешно удален" });
    }
}

public record UserRegistrationDto(string PhoneNumber, string Password, string DeviceId);
public record UserLoginDto(string PhoneNumber, string Password, string DeviceId);