namespace Geoportal.Data;

public class Report
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeviceId { get; set; }
    public string IpAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ImageHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}