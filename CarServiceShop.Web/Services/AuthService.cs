using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CarServiceShop.Web.Services;

public class AuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ProtectedLocalStorage _localStorage;
    private readonly ILogger<AuthService> _logger;
    private const string TokenKey = "authToken";
    private const string UserKey = "currentUser";

    public AuthService(
        IHttpClientFactory httpClientFactory,
        ProtectedLocalStorage localStorage,
        ILogger<AuthService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        _logger = logger;
    }

    public event Action? OnAuthStateChanged;

    public async Task<LoginResponse?> LoginAsync(string username, string password)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CarServiceShopAPI");
            var request = new { username, password };
            var response = await client.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null)
                {
                    await _localStorage.SetAsync(TokenKey, loginResponse.Token);
                    await _localStorage.SetAsync(UserKey, JsonSerializer.Serialize(loginResponse));
                    OnAuthStateChanged?.Invoke();
                    return loginResponse;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.DeleteAsync(TokenKey);
        await _localStorage.DeleteAsync(UserKey);
        OnAuthStateChanged?.Invoke();
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var result = await _localStorage.GetAsync<string>(TokenKey);
            return result.Success ? result.Value : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<LoginResponse?> GetCurrentUserAsync()
    {
        try
        {
            var result = await _localStorage.GetAsync<string>(UserKey);
            if (result.Success && !string.IsNullOrEmpty(result.Value))
            {
                return JsonSerializer.Deserialize<LoginResponse>(result.Value);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}
