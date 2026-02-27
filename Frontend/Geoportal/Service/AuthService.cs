using System.Net.Http.Json;

namespace GeoportalApp.Services;

public class AuthService
{
    private readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(10) };
    private const string BaseUrl = "http://136.113.150.143:5001/api/Auth";

    private string GetOrCreateDeviceId()
    {
        string id = Preferences.Default.Get("device_unique_id", string.Empty);

        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
            Preferences.Default.Set("device_unique_id", id);
        }
        return id;
    }

    public async Task<(bool Success, string Message)> LoginAsync(string phone, string password)
    {
        var request = new
        {
            PhoneNumber = phone,
            Password = password,
            DeviceId = GetOrCreateDeviceId()
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", request);
            if (response.IsSuccessStatusCode)
            {
                Preferences.Default.Set("is_logged_in", true);
                Preferences.Default.Set("user_phone", phone);
                return (true, "OK");
            }
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch { return (false, "Server error"); }
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string phone, string password)
    {
        var request = new
        {
            PhoneNumber = phone,
            Password = password,
            DeviceId = GetOrCreateDeviceId()
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register", request);
            return response.IsSuccessStatusCode ? (true, "OK") : (false, await response.Content.ReadAsStringAsync());
        }
        catch { return (false, "Server error"); }
    }
}