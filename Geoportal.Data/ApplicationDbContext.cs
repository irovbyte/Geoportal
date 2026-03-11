using Geoportal.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Geoportal.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Report> Reports { get; set; }
    public DbSet<InfraObject> InfraObjects { get; set; }
    public DbSet<User> Users { get; set; }
}
