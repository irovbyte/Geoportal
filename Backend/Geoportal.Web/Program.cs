using Geoportal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Настройка БД (пока можно закомментировать, если еще не настроил миграции)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Настройка конвейера
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles(); // ЭТО ОБЯЗАТЕЛЬНО для работы стилей и картинок
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages(); // Это связывает URL "/" с файлом Index.cshtml

app.Run();