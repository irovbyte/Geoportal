using Microsoft.EntityFrameworkCore;

namespace Geoportal.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Эта строка говорит базе данных: "Создай таблицу Reports на основе модели Report"
    public DbSet<Report> Reports { get; set; }
}