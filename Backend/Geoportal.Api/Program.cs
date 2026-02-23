using Geoportal.Data;
using Microsoft.EntityFrameworkCore;
using Geoportal.Data.Interfaces; 
using Geoportal.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка CORS (чтобы Flutter мог достучаться)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. Подключение БД Postgres
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Регистрация интерфейсов (Dependency Injection)
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IFileService, LocalFileService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Автоматическое создание БД и таблиц (если их еще нет)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// 5. Включаем CORS
app.UseCors("AllowAll");

// 6. Swagger (всегда включен для тестов на сервере)
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geoportal API V1");
    c.RoutePrefix = "swagger"; // Swagger будет доступен по адресу /swagger
});

app.UseAuthorization();
app.MapControllers();

app.Run();