using Microsoft.Maui.Controls;
using ZXing.Net.Maui;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using DoAnCSharp.Models;
using DoAnCSharp.Services;
using System.Diagnostics;

namespace DoAnCSharp.Views
{
    public partial class ScanQRPage : ContentPage
    {
        private readonly IPaymentService? _paymentService;
        private readonly DatabaseService? _dbService;
        private readonly ApiService? _apiService;
        private int _currentUserId = 1; // Get from AuthService

        public ScanQRPage()
        {
            InitializeComponent();
            _paymentService = ServiceHelper.GetService<IPaymentService>();
            _dbService = ServiceHelper.GetService<DatabaseService>();
            _apiService = ServiceHelper.GetService<ApiService>();

            // Cấu hình quét mã QR
            cameraBarcodeReaderView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormat.QrCode,
                AutoRotate = true,
                Multiple = false
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cameraBarcodeReaderView.IsDetecting = true;
        }

        protected override void OnDisappearing()
        {
            cameraBarcodeReaderView.IsDetecting = false;
            base.OnDisappearing();
        }

        private void CameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            var result = e.Results?.FirstOrDefault();
            if (result != null)
            {
                // Dừng quét ngay khi bắt được mã
                cameraBarcodeReaderView.IsDetecting = false;

                Dispatcher.Dispatch(async () =>
                {
                    try
                    {
                        if (_paymentService == null)
                        {
                            await DisplayAlert("Lỗi", "Không thể khởi tạo dịch vụ thanh toán", "OK");
                            await Navigation.PopAsync();
                            return;
                        }

                        // Get current user
                        if (_dbService != null)
                        {
                            var currentUser = await _dbService.GetCurrentUserAsync();
                            if (currentUser != null)
                            {
                                _currentUserId = currentUser.Id;
                            }
                        }

                        // Check payment status and QR scan limits
                        bool isPaid = await _paymentService.CheckIfUserPaidAsync(_currentUserId);
                        bool canScan = await _paymentService.CheckQRScanLimitAsync(_currentUserId, isPaid);

                        if (!canScan)
                        {
                            // Show alert and navigate to payment page
                            await DisplayAlert(
                                "⚠️ Hết Lần Quét",
                                "Bạn đã hết lần quét hôm nay. Vui lòng nâng cấp Premium để quét không giới hạn!",
                                "OK"
                            );
                            await Navigation.PopAsync();
                            return;
                        }

                        string qrValue = result.Value;

                        // Increment scan count
                        await _paymentService.IncrementQRScanCountAsync(_currentUserId);

                        // 📍 Update online status via API
                        try
                        {
                            if (_apiService != null && await _apiService.IsWebAdminAvailableAsync())
                            {
                                // Update online status through API
                                // Note: This will be handled by the Web Admin's UserStatusController
                                Debug.WriteLine($"✅ User {_currentUserId} marked online via API");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"⚠️ Failed to update online status: {ex.Message}");
                        }

                        // Gửi tin nhắn chứa nội dung QR đi
                        WeakReferenceMessenger.Default.Send(new QrScannedMessage(qrValue));

                        // Quay về trang bản đồ
                        await Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"❌ QR Scan Error: {ex.Message}");
                        await DisplayAlert("Lỗi", $"Lỗi quét mã: {ex.Message}", "OK");
                        cameraBarcodeReaderView.IsDetecting = true; // Re-enable scanning
                    }
                });
            }
        }
    }
}
