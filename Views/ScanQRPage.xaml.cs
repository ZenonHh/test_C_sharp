using Microsoft.Maui.Controls;
using ZXing.Net.Maui;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using DoAnCSharp.Models;
using DoAnCSharp.Services;

namespace DoAnCSharp.Views
{
    public partial class ScanQRPage : ContentPage
    {
        private readonly IPaymentService? _paymentService;
        private int _currentUserId = 1; // Get from AuthService

        public ScanQRPage()
        {
            InitializeComponent();
            _paymentService = ServiceHelper.GetService<IPaymentService>();

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
                    if (_paymentService == null)
                    {
                        await DisplayAlert("Lỗi", "Không thể khởi tạo dịch vụ thanh toán", "OK");
                        await Navigation.PopAsync();
                        return;
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

                    // Gửi tin nhắn chứa nội dung QR đi
                    WeakReferenceMessenger.Default.Send(new QrScannedMessage(qrValue));

                    // Quay về trang bản đồ
                    await Navigation.PopAsync();
                });
            }
        }
    }
}
