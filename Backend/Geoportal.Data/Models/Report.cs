namespace Geoportal.Data;

public class Report
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ImageHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}