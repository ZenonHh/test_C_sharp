using DoAnCSharp.Services;
using System.Diagnostics;

namespace DoAnCSharp.Views;

public partial class PaymentPage : ContentPage
{
    private readonly IPaymentService _paymentService;
    private readonly DatabaseService _dbService;
    private readonly ApiService _apiService;
    private int _currentUserId = 1;
    private bool _isPaid = false;

    public PaymentPage()
    {
        InitializeComponent();
        _paymentService = ServiceHelper.GetService<IPaymentService>();
        _dbService = ServiceHelper.GetService<DatabaseService>();
        _apiService = ServiceHelper.GetService<ApiService>();
        LoadPaymentStatus();
    }

    private async void LoadPaymentStatus()
    {
        try
        {
            // Lấy email từ Preferences
            string? email = Microsoft.Maui.Storage.Preferences.Default.Get("CurrentUserEmail", "");
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Lỗi", "Chưa đăng nhập", "OK");
                return;
            }

            // 🔌 Ưu tiên lấy từ Web API, nếu không được thì dùng local DB
            var user = await _apiService.GetUserByEmailAsync(email);

            if (user == null)
            {
                Debug.WriteLine("⚠️  Lấy user từ local database");
                user = await _dbService.GetCurrentUserAsync();
            }

            if (user == null)
            {
                await DisplayAlert("Lỗi", "Không tìm thấy user", "OK");
                return;
            }

            _currentUserId = user.Id;
            _isPaid = user.IsPaid;

            UpdatePaymentUI();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Không thể tải thông tin thanh toán: {ex.Message}", "OK");
        }
    }

    private void UpdatePaymentUI()
    {
        if (_isPaid)
        {
            PaymentStatusLabel.Text = "✅ Tài khoản Thanh Toán";
            PaidStatusContainer.IsVisible = true;
            FreeStatusContainer.IsVisible = false;
            QRLimitFrame.IsVisible = false;
            PaymentButton.Text = "✅ Bạn đã thanh toán";
            PaymentButton.IsEnabled = false;
            PaymentButton.BackgroundColor = Color.FromArgb("#95a5a6");
        }
        else
        {
            PaymentStatusLabel.Text = "🆓 Tài khoản Miễn Phí";
            PaidStatusContainer.IsVisible = false;
            FreeStatusContainer.IsVisible = true;
            QRLimitFrame.IsVisible = true;
            PaymentButton.IsEnabled = true;
            PaymentButton.BackgroundColor = Color.FromArgb("#27ae60");
            PaymentButton.Text = "💳 Thanh Toán Ngay";
            LoadQRScanLimits();
        }
    }

    private async Task LoadQRScanLimits()
    {
        try
        {
            var canScan = await _paymentService.CheckQRScanLimitAsync(_currentUserId, false);

            // For demo purposes, we'll show the limit
            int scanCount = 4; // Example: 4 scans today
            int maxScans = 5;

            ScanProgressBar.Progress = (double)scanCount / maxScans;
            ScanProgressLabel.Text = $"{scanCount} / {maxScans} lần";
            ScanLimitLabel.Text = $"Còn {maxScans - scanCount} lần quét/ngày";

            if (scanCount >= maxScans)
            {
                ScanLimitLabel.TextColor = Color.FromArgb("#e74c3c");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Không thể tải giới hạn quét: {ex.Message}", "OK");
        }
    }

    private async void OnPaymentButtonClicked(object sender, EventArgs e)
    {
        if (_isPaid)
        {
            await DisplayAlert("Thông Báo", "Bạn đã là thành viên Premium! 🎉", "OK");
            return;
        }

        // Show payment methods
        string action = await DisplayActionSheet(
            "Chọn phương thức thanh toán",
            "Hủy",
            null,
            new[] { "💳 Thẻ Tín Dụng", "📱 Momo/ZaloPay", "🏦 Chuyển Khoản" }
        );

        if (action == "Hủy" || string.IsNullOrEmpty(action))
            return;

        // 💳 THANH TOÁN NGAY - Lưu trạng thái vào database
        bool isSuccess = false;

        if (await _apiService.IsWebAdminAvailableAsync())
        {
            Debug.WriteLine("✅ Cập nhật trạng thái thanh toán lên Web API");
            isSuccess = await _apiService.UpdatePaymentStatusAsync(_currentUserId, true);
        }
        else
        {
            Debug.WriteLine("⚠️  Cập nhật trạng thái thanh toán trên local database");
            isSuccess = await _dbService.UpdatePaymentStatusAsync(_currentUserId, true);
        }

        if (isSuccess)
        {
            _isPaid = true;
            await DisplayAlert("✅ Thành Công", "Cảm ơn bạn! Tài khoản của bạn đã được nâng cấp. 🎉", "OK");
            UpdatePaymentUI();
        }
        else
        {
            await DisplayAlert("❌ Lỗi", "Thanh toán thất bại, vui lòng thử lại", "OK");
        }
    }
}
