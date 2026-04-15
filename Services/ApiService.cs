using DoAnCSharp.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Net.Http.Json;

namespace DoAnCSharp.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private string _baseUrl = "http://localhost:5000/api";
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ApiService(string? baseUrl = null)
    {
        _httpClient = new HttpClient();
        if (!string.IsNullOrEmpty(baseUrl))
            _baseUrl = baseUrl;
    }

    public void SetBaseUrl(string baseUrl) => _baseUrl = baseUrl;

    // ==================== QUÁN ĂN (POI) ====================
    
    public async Task<List<AudioPOI>> GetPOIsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/pois");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<AudioPOI>>(json, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ GetPOIs Error: {ex.Message}");
            return new();
        }
    }

    public async Task<AudioPOI?> GetPOIByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/pois/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AudioPOI>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ GetPOI {id} Error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreatePOIAsync(AudioPOI poi)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/pois", poi);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ CreatePOI Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdatePOIAsync(int id, AudioPOI poi)
    {
        try
        {
            poi.Id = id;
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/pois/{id}", poi);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ UpdatePOI Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeletePOIAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/pois/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ DeletePOI Error: {ex.Message}");
            return false;
        }
    }

    // ==================== NGƯỜI DÙNG ====================
    
    public async Task<List<User>> GetUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/users");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(json, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ GetUsers Error: {ex.Message}");
            return new();
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            var users = await GetUsersAsync();
            return users.FirstOrDefault(u => u.Email == email);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ GetUserByEmail Error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> RegisterUserAsync(string fullName, string email, string password)
    {
        try
        {
            var user = new User
            {
                FullName = fullName,
                Email = email,
                Password = password,
                Avatar = "dotnet_bot.png",
                Phone = "Đang cập nhật"
            };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/users", user);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ RegisterUser Error: {ex.Message}");
            return false;
        }
    }

    public async Task<User?> LoginUserAsync(string email, string password)
    {
        try
        {
            var user = await GetUserByEmailAsync(email);
            if (user != null && user.Password == password)
                return user;
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ LoginUser Error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateUserAsync(int id, User user)
    {
        try
        {
            user.Id = id;
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/users/{id}", user);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ UpdateUser Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/users/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ DeleteUser Error: {ex.Message}");
            return false;
        }
    }

    // ==================== LỊCH SỬ PHÁT ====================
    
    public async Task<List<PlayHistory>> GetPlayHistoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/history");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PlayHistory>>(json, _jsonOptions) ?? new();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ GetPlayHistories Error: {ex.Message}");
            return new();
        }
    }

    public async Task<List<PlayHistory>> GetRecentPlayHistoryAsync()
    {
        try
        {
            var all = await GetPlayHistoriesAsync();
            return all.OrderByDescending(x => x.PlayedAt).Take(10).ToList();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ GetRecentPlayHistory Error: {ex.Message}");
            return new();
        }
    }

    // ==================== KIỂM TRA KẾT NỐI ====================
    
    public async Task<bool> IsWebAdminAvailableAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/pois");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
