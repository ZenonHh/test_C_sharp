using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoAnCSharp.Services;
using DoAnCSharp;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DoAnCSharp.ViewModels;

public partial class AuthViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly DatabaseService _dbService;

    [ObservableProperty]
#pragma warning disable MVVMTK0045 // Using [ObservableProperty] on fields is not AOT compatible for WinRT
    private string _email = string.Empty;
#pragma warning restore MVVMTK0045

    [ObservableProperty]
#pragma warning disable MVVMTK0045
    private string _password = string.Empty;
#pragma warning restore MVVMTK0045

    public AuthViewModel(IAuthService authService, DatabaseService dbService)
    {
        _authService = authService;
        _dbService = dbService; 
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

        // 2. Lưu Email người dùng vừa nhập vào bộ nhớ cục bộ của điện thoại
        Microsoft.Maui.Storage.Preferences.Default.Set("CurrentUserEmail", Email);

        // 3. Chạy xuống SQLite kiểm tra xem có tài khoản này chưa, chưa thì tạo
        await _dbService.GetOrCreateUserAsync(Email);

        // 4. Lưu trạng thái đăng nhập
        if (_authService != null)
        {
            await _authService.SetLoggedInAsync(true);
        }

        // 5. CHUYỂN GIAO DIỆN: Nạp khung AppShell và nhảy sang Tab Bản đồ
        if (Application.Current != null)
        {
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//MapTab");
        }
    }
}