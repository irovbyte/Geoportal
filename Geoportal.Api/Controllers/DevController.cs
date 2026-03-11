namespace Geoportal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DevController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("seed-data")]
    public async Task<IActionResult> SeedMockData()
    {
        if (_context.InfraObjects.Any()) return Ok("Данные уже существуют!");

        var random = new Random();
        var regions = new[] { "Toshkent", "Samarqand", "Farg'ona", "Andijon", "Navoiy" };
        var sectors = new[] { "Maktab", "Bog'cha", "Tibbiyot", "Sport" };
        var donors = new[] { "Davlat byudjeti", "Jahon Banki", "Xususiy investor", "Osiyo Taraqqiyot Banki" };

        var mockData = new List<InfraObject>();

        for (int i = 1; i <= 500; i++)
        {
            var sector = sectors[random.Next(sectors.Length)];
            var region = regions[random.Next(regions.Length)];

            double lat = 39.0 + random.NextDouble() * 3.0;
            double lng = 64.0 + random.NextDouble() * 8.0;

            mockData.Add(new InfraObject
            {
                Name = $"{region} {random.Next(1, 100)}-{sector.ToLower()}",
                Region = region,
                District = "Markaziy tuman",
                Sector = sector,
                Latitude = lat,
                Longitude = lng,
                AllocatedBudget = random.Next(1000, 50000) * 1000000m,
                SpentBudget = random.Next(500, 45000) * 1000000m,
                CompletionPercentage = random.Next(10, 100),
                DonorName = donors[random.Next(donors.Length)],
                Capacity = random.Next(100, 2000),
                CurrentPeopleCount = random.Next(80, 1900),
                HasInternet = random.Next(100) > 20,
                HasDrinkingWater = random.Next(100) > 10
            });
        }

        await _context.InfraObjects.AddRangeAsync(mockData);
        await _context.SaveChangesAsync();

        return Ok($"Успешно сгенерировано 500 объектов!");
    }
}
