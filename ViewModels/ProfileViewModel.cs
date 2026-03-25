using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoAnCSharp.Models;
using DoAnCSharp.Services;
using System; // Bắt buộc phải có để dùng IServiceProvider
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DoAnCSharp.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private readonly IServiceProvider _serviceProvider; // Khai báo công cụ "lấy" trang

    [ObservableProperty]
    private User? _currentUser;

    // Sửa constructor: Nhận thêm IServiceProvider từ hệ thống
    public ProfileViewModel(DatabaseService dbService, IServiceProvider serviceProvider)
    {
        _dbService = dbService;
        _serviceProvider = serviceProvider;
    }

    public async Task LoadUserProfileAsync()
    {
        CurrentUser = await _dbService.GetCurrentUserAsync();
    }

    [RelayCommand]
    private void Logout() // Bỏ async Task đi vì đoạn dưới không cần await
    {
        // Chuyển trang đúng chuẩn DI (Không dùng chữ "new")
        if (Application.Current != null)
        {
            var authPage = _serviceProvider.GetService(typeof(Views.AuthPage)) as Views.AuthPage;
            Application.Current.MainPage = authPage;
        }
    }

    [RelayCommand]
    private async Task ChangeLanguage()
    {
        // Tạm thời hiển thị thông báo. Bạn có thể gọi ILanguageService vào đây sau để xử lý đổi ngôn ngữ thật.
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert("Đổi ngôn ngữ", "Tính năng đổi ngôn ngữ trong trang Cá nhân đang được hoàn thiện!", "OK");
        }
    }

    [RelayCommand]
    private async Task EditProfile()
    {
        if (Application.Current?.MainPage != null)
            await Application.Current.MainPage.DisplayAlert("Thông báo", "Chức năng chỉnh sửa đang phát triển!", "OK");
    }
}