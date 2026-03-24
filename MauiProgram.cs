using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using DoAnCSharp.Services;
using DoAnCSharp.Views;
using DoAnCSharp.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace DoAnCSharp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 1. ĐĂNG KÝ DỊCH VỤ (Chỉ giữ lại những file thực sự tồn tại trong thư mục Services)
        builder.Services.AddSingleton<ILanguageService, LanguageService>();
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<LocationService>(); // Sửa lỗi Ambiguous
        builder.Services.AddSingleton<IAuthService, AuthService>();

        // 2. ĐĂNG KÝ VIEWMODELS (Chỉ giữ lại những file đang có)
        builder.Services.AddTransient<AuthViewModel>();
        builder.Services.AddSingleton<HomeViewModel>();

        // 3. ĐĂNG KÝ CÁC TRANG 
        builder.Services.AddTransient<Views.AuthPage>();
        builder.Services.AddSingleton<Views.HomePage>();
        builder.Services.AddSingleton<Views.MapPage>();
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        //builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}