using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;
using System.Text.Json;

namespace DoAnCSharp.Services;

public interface IPaymentService
{
    Task<bool> CheckIfUserPaidAsync(int userId);
    Task<PaymentInfo> GetUserPaymentInfoAsync(int userId);
    Task<bool> CheckQRScanLimitAsync(int userId, bool isPaid);
    Task IncrementQRScanCountAsync(int userId);
}

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private const string ApiBase = "http://10.0.2.2:5000/api"; // Android emulator localhost

    public PaymentService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<bool> CheckIfUserPaidAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBase}/payments/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var payment = JsonSerializer.Deserialize<PaymentResponse>(json);
                return payment?.IsPaid ?? false;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<PaymentInfo> GetUserPaymentInfoAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBase}/payments/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var payment = JsonSerializer.Deserialize<PaymentResponse>(json);
                return new PaymentInfo
                {
                    IsPaid = payment?.IsPaid ?? false,
                    Amount = payment?.Amount ?? 0,
                    PaidDate = payment?.PaidDate ?? DateTime.Now,
                    PaymentMethod = payment?.PaymentMethod ?? "None"
                };
            }
            return new PaymentInfo { IsPaid = false };
        }
        catch
        {
            return new PaymentInfo { IsPaid = false };
        }
    }

    public async Task<bool> CheckQRScanLimitAsync(int userId, bool isPaid)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBase}/qrscans/check/{userId}/{isPaid}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<QRScanCheckResponse>(json);
                return result?.CanScan ?? false;
            }
            return false;
        }
        catch
        {
            return isPaid; // If error, allow paid users only
        }
    }

    public async Task IncrementQRScanCountAsync(int userId)
    {
        try
        {
            await _httpClient.PostAsync($"{ApiBase}/qrscans/increment/{userId}", null);
        }
        catch
        {
            // Silently fail
        }
    }

   
  
}

public class PaymentResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsPaid { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidDate { get; set; } = DateTime.Now;
    public string PaymentMethod { get; set; } = string.Empty;
}

public class PaymentInfo
{
    public bool IsPaid { get; set; } = false;
    public decimal Amount { get; set; } = 0m;
    public DateTime PaidDate { get; set; } = DateTime.Now;
    public string PaymentMethod { get; set; } = string.Empty;
}

public class QRScanCheckResponse
{
    public bool CanScan { get; set; }
    public int ScanCount { get; set; }
    public string MaxScans { get; set; } = "5";
    public int RemainingScansFree { get; set; }
    public bool IsPaidUser { get; set; }
}
