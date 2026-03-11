public class LoginRequest
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
}

public class RegisterRequest : LoginRequest
{
    public string FullName { get; set; } = string.Empty;
}