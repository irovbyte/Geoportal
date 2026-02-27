using Geoportal.Data;
using Microsoft.EntityFrameworkCore;
// ИСПРАВЛЕНИЕ: Подключаем пространства имен из проекта Data, а не Api
using Geoportal.Data.Interfaces;
using Geoportal.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(_ => true);
    });
});

// 2. Подключение БД Postgres
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Регистрация сервисов (Dependency Injection)
// Теперь он найдет эти классы, так как мы добавили правильные using выше
builder.Services.AddScoped<IReportRepository, SqlReportRepository>();
builder.Services.AddScoped<IFileService, LocalFileService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Авто-создание БД
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors("AllowAll");

// 5. Swagger всегда включен
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geoportal API V1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();
app.MapControllers();

app.Run();