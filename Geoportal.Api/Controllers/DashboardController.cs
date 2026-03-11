namespace Geoportal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public DashboardController(ApplicationDbContext context) => _context = context;

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var totalAllocated = await _context.InfraObjects.SumAsync(x => x.AllocatedBudget);
        var totalSpent = await _context.InfraObjects.SumAsync(x => x.SpentBudget);
        var totalProjects = await _context.InfraObjects.CountAsync();
        var avgCompletion = await _context.InfraObjects.AverageAsync(x => x.CompletionPercentage);

        // Топ доноров
        var topDonors = await _context.InfraObjects
            .GroupBy(x => x.DonorName)
            .Select(g => new { Donor = g.Key, TotalInvested = g.Sum(x => x.AllocatedBudget) })
            .OrderByDescending(x => x.TotalInvested)
            .Take(5)
            .ToListAsync();

        // Топ регионов
        var topRegions = await _context.InfraObjects
            .GroupBy(x => x.Region)
            .Select(g => new { Region = g.Key, ProjectCount = g.Count() })
            .OrderByDescending(x => x.ProjectCount)
            .Take(5)
            .ToListAsync();

        return Ok(new {
            Totals = new {
                Allocated = totalAllocated,
                Spent = totalSpent,
                ProjectsCount = totalProjects,
                Completion = Math.Round(avgCompletion, 1)
            },
            TopDonors = topDonors,
            TopRegions = topRegions
        });
    }
}
