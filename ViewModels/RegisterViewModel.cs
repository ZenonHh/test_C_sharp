#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoAnCSharp.Helpers;
using DoAnCSharp.Services;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DoAnCSharp.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string _fullName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private string _error = string.Empty;

    private string _selectedAvatarPath = "dotnet_bot.png";

    [ObservableProperty]
    private ImageSource _avatarSource = ImageSource.FromFile("dotnet_bot.png");

    public RegisterViewModel(IAuthService authService, IServiceProvider serviceProvider)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    private async Task Register()
    {
        if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Vui lòng nhập đầy đủ thông tin";
            return;
        }

        if (Password != ConfirmPassword)
        {
            Error = "Mật khẩu không trùng khớp";
            return;
        }

        Error = string.Empty;

        bool isSuccess = await _authService.RegisterAsync(Email, Password, FullName, _selectedAvatarPath);

        if (isSuccess)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Thành công", "Đăng ký thành công! Mời bạn đăng nhập.", "OK");
                // Chuyển về trang AuthPage
                Application.Current.MainPage = _serviceProvider.GetService(typeof(Views.AuthPage)) as Views.AuthPage;
            }
        }
        else
        {
            Error = "Đăng ký thất bại. Email có thể đã tồn tại.";
        }
    }

    [RelayCommand]
    private void GoToLogin()
    {
        // Xử lý nút quay về trang Đăng nhập
        if (Application.Current != null)
        {
            Application.Current.MainPage = _serviceProvider.GetService(typeof(Views.AuthPage)) as Views.AuthPage;
        }
    }

    [RelayCommand]
    private async Task SelectAvatarSource(string sourceType)
    {
        try
        {
            FileResult photoResult = null;

            if (sourceType == "camera")
            {
                if (!await PermissionHelper.RequestCameraPermissionAsync())
                {
                    Error = "Chưa cấp quyền Camera.";
                    return;
                }
                photoResult = await MediaPicker.Default.CapturePhotoAsync();
            }
            else if (sourceType == "library")
            {
                if (!await PermissionHelper.RequestMediaPermissionAsync())
                {
                    Error = "Chưa cấp quyền truy cập Thư viện.";
                    return;
                }
                photoResult = await MediaPicker.Default.PickPhotoAsync();
            }

            if (photoResult != null)
            {
                var stream = await photoResult.OpenReadAsync();
                AvatarSource = ImageSource.FromStream(() => stream);
                Error = string.Empty;

                await SaveImageLocalAsync(photoResult);
            }
        }
        catch (Exception ex)
        {
            Error = $"Lỗi: {ex.Message}";
        }
    }

    private async Task SaveImageLocalAsync(FileResult photo)
    {
        try
        {
            string newFileName = $"avatar_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            string destinationPath = Path.Combine(FileSystem.AppDataDirectory, newFileName);

            using (Stream sourceStream = await photo.OpenReadAsync())
            using (FileStream destStream = File.Create(destinationPath))
            {
                await sourceStream.CopyToAsync(destStream);
            }

            _selectedAvatarPath = destinationPath;
        }
        catch { }
    }
}