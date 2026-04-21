using DoAnCSharp.Views;
using DoAnCSharp.Services;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace DoAnCSharp;

public partial class App : Application
{
    // ✅ Vào trực tiếp MapPage (skip AuthPage)
    public App(AppShell appShell, DatabaseService dbService, ILanguageService langService)
    {
        InitializeComponent(); 

        // Khởi tạo ngôn ngữ đã chọn (từ lần trước)
        langService.Initialize();

        // Tạo bảng và nạp dữ liệu mẫu
        Task.Run(async () => await dbService.SeedDataAsync());

        // ✅ Vào thẳng AppShell (chứa MapPage)
        MainPage = appShell;
    }
}