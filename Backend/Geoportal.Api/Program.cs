using Geoportal.Data;
using Geoportal.Data.Interfaces;
using Geoportal.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Указываем порт
builder.WebHost.UseUrls("http://0.0.0.0:5001");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// РЕГИСТРАЦИЯ РЕПОЗИТОРИЯ
// Если на хакатоне будет Excel, просто заменишь SqlReportRepository на ExcelReportRepository тут
builder.Services.AddScoped<IReportRepository, SqlReportRepository>();
builder.Services.AddScoped<IFileService, LocalFileService>();


// БАЗА ДАННЫХ
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Авто-создание таблиц
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();

app.Run();