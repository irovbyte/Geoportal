namespace Geoportal.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
{
    try
    {
        bool hasData = await _context.InfraObjects.AnyAsync();

        var totalAllocated = hasData ? await _context.InfraObjects.SumAsync(x => x.AllocatedBudget) : 0;
        var totalSpent = hasData ? await _context.InfraObjects.SumAsync(x => x.SpentBudget) : 0;
        var totalProjects = await _context.InfraObjects.CountAsync();
        var avgCompletion = hasData
            ? await _context.InfraObjects.AverageAsync(x => (double)x.CompletionPercentage)
            : 0;

        ViewBag.MaktablarCount = await _context.InfraObjects.CountAsync(x => x.Sector == "Maktab");
        ViewBag.BogchalarCount = await _context.InfraObjects.CountAsync(x => x.Sector == "Bog'cha");
        ViewBag.TibbiyotCount = await _context.InfraObjects.CountAsync(x => x.Sector == "Tibbiyot");
        ViewBag.SportCount = await _context.InfraObjects.CountAsync(x => x.Sector == "Sport");

        var topDonors = await _context.InfraObjects
            .GroupBy(x => x.DonorName)
            .Select(g => new TopDonorDto {
                Donor = g.Key ?? "Noma'lum",
                TotalInvested = g.Sum(x => x.AllocatedBudget)
            })
            .OrderByDescending(x => x.TotalInvested)
            .Take(3)
            .ToListAsync();

        ViewBag.TotalAllocated = totalAllocated;
        ViewBag.TotalSpent = totalSpent;
        ViewBag.TotalProjects = totalProjects;
        ViewBag.AvgCompletion = Math.Round(avgCompletion, 1);
        ViewBag.TopDonors = topDonors;

        return View();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ошибка в Dashboard: " + ex.Message);
        return Content("Ошибка базы данных. Проверьте логи.");
    }
}
    public IActionResult Graphics() => View();
    public IActionResult Report() => View();
    public IActionResult Submit() => View();
    public IActionResult Maktablar() => View();
    public IActionResult Bogchalar() => View();
    public IActionResult Tibbiyot() => View();
    public IActionResult Sport() => View();
    public IActionResult Download() => View();
    public IActionResult Login() => View();
}
public class TopDonorDto {
    public string Donor { get; set; }
    public decimal TotalInvested { get; set; }
}
