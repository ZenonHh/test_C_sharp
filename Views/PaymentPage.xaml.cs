using DoAnCSharp.Services;

namespace DoAnCSharp.Views;

public partial class PaymentPage : ContentPage
{
    private readonly IPaymentService _paymentService;
    private int _currentUserId = 1; // You should get this from AuthService
    private bool _isPaid = false;

    public PaymentPage()
    {
        InitializeComponent();
        _paymentService = ServiceHelper.GetService<IPaymentService>();
        LoadPaymentStatus();
    }

    private async void LoadPaymentStatus()
    {
        try
        {
            // Get current user ID from AuthService
            var authService = ServiceHelper.GetService<IAuthService>();
            if (authService != null)
            {
                // Assuming AuthService has a method to get current user ID
                _currentUserId = 1; // Update this with actual user ID
            }

            // Check payment status
            _isPaid = await _paymentService.CheckIfUserPaidAsync(_currentUserId);
            
            if (_isPaid)
            {
                // User is paid
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
                // User is free
                PaymentStatusLabel.Text = "🆓 Tài khoản Miễn Phí";
                PaidStatusContainer.IsVisible = false;
                FreeStatusContainer.IsVisible = true;
                QRLimitFrame.IsVisible = true;
                PaymentButton.IsEnabled = true;
                PaymentButton.BackgroundColor = Color.FromArgb("#27ae60");

                // Load QR scan limits
                await LoadQRScanLimits();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Không thể tải thông tin thanh toán: {ex.Message}", "OK");
        }
    }

    private async Task LoadQRScanLimits()
    {
        try
        {
            var canScan = await _paymentService.CheckQRScanLimitAsync(_currentUserId, false);
            
            // For demo purposes, we'll show the limit
            // In real app, you'd fetch actual scan count from API
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

        switch (action)
        {
            case "💳 Thẻ Tín Dụng":
                await DisplayAlert("Thanh Toán", "Sẽ chuyển đến trang thanh toán thẻ...", "OK");
                // TODO: Integrate payment gateway
                break;
            case "📱 Momo/ZaloPay":
                await DisplayAlert("Thanh Toán", "Sẽ chuyển đến Momo/ZaloPay...", "OK");
                // TODO: Integrate mobile payment
                break;
            case "🏦 Chuyển Khoản":
                await DisplayAlert("Thông Tin Chuyển Khoản", 
                    "Ngân hàng: Vietcombank\n" +
                    "Số TK: 1234567890\n" +
                    "Tên: Vĩnh Khánh Tour\n\n" +
                    "Sau khi chuyển, liên hệ support để kích hoạt tài khoản.", "OK");
                break;
        }
    }
}
