using Geoportal.Data;
using Geoportal.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Geoportal.Data.Repositories;

public class SqlReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _context;

    public SqlReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Report>> GetAllAsync() => await _context.Reports.ToListAsync();

    public async Task<Report?> GetByIdAsync(Guid id) => await _context.Reports.FindAsync(id);

    public async Task AddAsync(Report report) => await _context.Reports.AddAsync(report);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}