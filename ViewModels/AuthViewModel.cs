using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoAnCSharp.Services;
using DoAnCSharp.Models;
using DoAnCSharp;
using System; 
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace DoAnCSharp.ViewModels;

public partial class AuthViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly DatabaseService _dbService;
    private readonly ApiService _apiService;  // 🔌 Thêm ApiService
    private readonly IServiceProvider _serviceProvider; // Khai báo công cụ chuyển trang

    [ObservableProperty]
#pragma warning disable MVVMTK0045
    private string _email = string.Empty;
#pragma warning restore MVVMTK0045

    [ObservableProperty]
#pragma warning disable MVVMTK0045
    private string _password = string.Empty;
#pragma warning restore MVVMTK0045

    // Phải nhận cả 4 công cụ vào Constructor
    public AuthViewModel(IAuthService authService, DatabaseService dbService, ApiService apiService, IServiceProvider serviceProvider)
    {
        _authService = authService;
        _dbService = dbService;
        _apiService = apiService;  // 🔌 Inject ApiService
        _serviceProvider = serviceProvider;
    }
    [RelayCommand]
    private async Task Login()
    {
        // 1. Kiểm tra dữ liệu rỗng
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Thông báo", "Vui lòng nhập email và mật khẩu!", "OK");
            return;
        }

        // Chuẩn hóa email
        string normalizedEmail = Email.Trim().ToLower();

        // 🔌 Ưu tiên lấy từ Web API, nếu không được thì dùng local DB
        User? user = null;

        if (await _apiService.IsWebAdminAvailableAsync())
        {
            Debug.WriteLine("✅ Đăng nhập qua Web API");
            user = await _apiService.LoginUserAsync(normalizedEmail, Password);
        }
        else
        {
            Debug.WriteLine("⚠️  Đăng nhập qua local database");
            user = await _dbService.LoginUserAsync(normalizedEmail, Password);
        }

        if (user != null)
        {
            Microsoft.Maui.Storage.Preferences.Default.Set("CurrentUserEmail", normalizedEmail);

            if (_authService != null)
            {
                await _authService.SetLoggedInAsync(true);
            }

            if (Application.Current != null)
            {
                var appShell = _serviceProvider.GetService(typeof(AppShell)) as AppShell;

                if (appShell != null)
                {
                    Application.Current.MainPage = appShell;
                    await Task.Delay(100);
                    await Shell.Current.GoToAsync("//MapTab");
                }
            }
        }
        else
        {
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Sai Email hoặc Mật khẩu!\nVui lòng kiểm tra lại.", "OK");
        }
    }
    // Hàm để bấm nút "Đăng ký ngay" chuyển sang trang RegisterPage
    [RelayCommand]
    private void GoToRegister()
    {
        if (Application.Current != null)
        {
            // Lấy trang RegisterPage ra và chuyển qua
            var registerPage = _serviceProvider.GetService(typeof(Views.RegisterPage)) as Views.RegisterPage;
            if (registerPage != null)
            {
                Application.Current.MainPage = registerPage;
            }
        }
    }
}