namespace Geoportal.Models
{
    public class LoginRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
    }
}
