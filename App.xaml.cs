using DoAnCSharp.Views;
using DoAnCSharp.Services;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace DoAnCSharp;

public partial class App : Application
{
    // ĐÃ SỬA: Thêm ILanguageService vào tham số
    public App(AuthPage authPage, DatabaseService dbService, ILanguageService langService)
    {
        InitializeComponent(); 

        // Khởi tạo ngôn ngữ đã chọn (từ lần trước) ngay khi vừa mở app
        langService.Initialize();

        // Chạy ngầm việc tạo bảng và nạp dữ liệu mẫu
        Task.Run(async () => await dbService.SeedDataAsync());

        // Đặt màn hình khởi động là trang Đăng nhập
        MainPage = authPage;
    }
}