namespace Geoportal.Data;

public class Report
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DeviceId { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string InfraName { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string ImageHash { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Status { get; set; } = "Yangi";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
