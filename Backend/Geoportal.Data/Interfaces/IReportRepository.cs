using Geoportal.Data;

namespace Geoportal.Data.Interfaces;

public interface IReportRepository
{
    Task<IEnumerable<Report>> GetAllAsync();
    Task<Report?> GetByIdAsync(Guid id);
    Task AddAsync(Report report);
    Task SaveChangesAsync();
}