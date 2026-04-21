using System.Diagnostics;

namespace DoAnCSharp.Services;

/// <summary>
/// Configuration helper for API connectivity
/// Manages different base URLs for dev, staging, and production
/// </summary>
public static class ApiConfiguration
{
    /// <summary>
    /// Current environment (change before building release)
    /// </summary>
    public static AppEnvironment CurrentEnvironment { get; set; } = AppEnvironment.Development;

    /// <summary>
    /// Get API base URL based on current environment
    /// </summary>
    public static string GetBaseUrl()
    {
        return CurrentEnvironment switch
        {
            AppEnvironment.Development => GetDevelopmentUrl(),
            AppEnvironment.Staging => "https://staging-api.vinhkhanhtour.com/api",
            AppEnvironment.Production => "https://api.vinhkhanhtour.com/api",
            _ => GetDevelopmentUrl()
        };
    }

    /// <summary>
    /// Get development URL based on platform
    /// </summary>
    private static string GetDevelopmentUrl()
    {
#if ANDROID
        // Android emulator: 10.0.2.2 = localhost of host machine
        return "http://10.0.2.2:5000/api";
#elif IOS
        // iOS simulator can use localhost
        return "http://localhost:5000/api";
#elif WINDOWS || MACCATALYST
        // Windows/Mac can use localhost
        return "http://localhost:5000/api";
#else
        // Default for other platforms
        return "http://localhost:5000/api";
#endif
    }

    /// <summary>
    /// Get development URL for physical device
    /// IMPORTANT: Replace with your actual machine IP when testing on physical device
    /// </summary>
    /// <param name="machineIp">Your machine's IP address (e.g., "192.168.1.100")</param>
    public static string GetPhysicalDeviceUrl(string machineIp)
    {
        return $"http://{machineIp}:5000/api";
    }

    /// <summary>
    /// Test if API is reachable
    /// </summary>
    public static async Task<bool> TestConnectionAsync()
    {
        try
        {
            var baseUrl = GetBaseUrl();
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            var response = await client.GetAsync($"{baseUrl.Replace("/api", "")}/api/pois");
            
            var isSuccess = response.IsSuccessStatusCode;
            
            Debug.WriteLine($"🌐 API Connection Test:");
            Debug.WriteLine($"   URL: {baseUrl}");
            Debug.WriteLine($"   Status: {(isSuccess ? "✅ SUCCESS" : "❌ FAILED")}");
            Debug.WriteLine($"   Response: {response.StatusCode}");
            
            return isSuccess;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ API Connection Failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Get QR scan landing URL
    /// </summary>
    public static string GetQRScanUrl(string qrCode)
    {
        var baseUrl = GetBaseUrl().Replace("/api", "");
        return $"{baseUrl}/qr-scan?code={qrCode}";
    }

    /// <summary>
    /// Log current configuration
    /// </summary>
    public static void LogConfiguration()
    {
        Debug.WriteLine("╔════════════════════════════════════════════════╗");
        Debug.WriteLine("║      API CONFIGURATION                         ║");
        Debug.WriteLine("╠════════════════════════════════════════════════╣");
        Debug.WriteLine($"║ Environment: {CurrentEnvironment,-33} ║");
        Debug.WriteLine($"║ Base URL:    {GetBaseUrl(),-33} ║");
        Debug.WriteLine($"║ Platform:    {DeviceInfo.Platform,-33} ║");
        Debug.WriteLine($"║ Device Type: {DeviceInfo.DeviceType,-33} ║");
        Debug.WriteLine("╚════════════════════════════════════════════════╝");
    }
}

/// <summary>
/// Application environment types
/// </summary>
public enum AppEnvironment
{
    /// <summary>
    /// Local development (localhost or 10.0.2.2)
    /// </summary>
    Development,
    
    /// <summary>
    /// Staging server for testing
    /// </summary>
    Staging,
    
    /// <summary>
    /// Production server
    /// </summary>
    Production
}
