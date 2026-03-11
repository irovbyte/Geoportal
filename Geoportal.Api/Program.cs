using Geoportal.Data;
using Microsoft.EntityFrameworkCore;
using Geoportal.Data.Interfaces;
using Geoportal.Data.Repositories;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка CORS
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

// 3. Регистрация сервисов
builder.Services.AddScoped<IReportRepository, SqlReportRepository>();
builder.Services.AddScoped<IFileService, LocalFileService>();

// Настройка контроллеров с подавлением ошибок типизации JSON для Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();

// ИСПРАВЛЕНИЕ: Явное указание документа v1 для .NET 10
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Geoportal API",
        Version = "v1",
        Description = "Geoportal Backend Service on .NET 10"
    });
});

var app = builder.Build();

// 4. Авто-создание БД
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors("AllowAll");

// 5. Swagger
app.UseSwagger(c => {
    // Это гарантирует, что формат будет соответствовать ожиданиям UI
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geoportal API V1");
    // Если хочешь, чтобы Swagger был главной страницей (api.site.uz/), оставь RoutePrefix пустым
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();
app.MapControllers();

app.Run();
