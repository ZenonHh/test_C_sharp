using Microsoft.Maui.Controls;
using ZXing.Net.Maui;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using DoAnCSharp.Models;

namespace DoAnCSharp.Views
{
    public partial class ScanQRPage : ContentPage
    {
        public ScanQRPage()
        {
            InitializeComponent();
            
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
        // 1. Dừng quét ngay để tránh lặp
        cameraBarcodeReaderView.IsDetecting = false;
        
        // 2. Rung nhẹ một cái cho "pro"
        try { Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100)); } catch { }

        Dispatcher.Dispatch(async () =>
        {
            string qrValue = result.Value; // Đây là tên quán từ mã QR
            
            // 3. Gửi tin nhắn chứa tên quán về MapPage
            WeakReferenceMessenger.Default.Send(new QrScannedMessage(qrValue));
            
            // 4. Quay lại trang bản đồ
            await Navigation.PopAsync();
        });
    }
}
    }
}