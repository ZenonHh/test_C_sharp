using DoAnCSharp.Views;
using DoAnCSharp.Services;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace DoAnCSharp;

public partial class App : Application
{
    // Bỏ các chữ "Views." và "Services.", chỉ nhận tên Class nhờ có các dòng using ở trên
    public App(AuthPage authPage, DatabaseService dbService)
    {
        InitializeComponent(); // Giờ dòng này sẽ sáng lên, hết báo lỗi!

        // Chạy ngầm việc tạo bảng và nạp dữ liệu mẫu
        Task.Run(async () => await dbService.SeedDataAsync());

        // Đặt màn hình khởi động
        MainPage = authPage;
    }
}