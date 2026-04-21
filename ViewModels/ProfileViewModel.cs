using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoAnCSharp.Models;
using DoAnCSharp.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace DoAnCSharp.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private readonly ApiService _apiService;
    private readonly IServiceProvider _serviceProvider;

    // ĐÂY CHÍNH LÀ BIẾN "Lang" ĐỂ GIAO DIỆN LẤY CHỮ DỊCH
    public ILanguageService Lang { get; }

    [ObservableProperty]
    private User? _currentUser;

    [ObservableProperty]
    private bool _isLoggedIn = false;

    // Tiêm các Service vào
    public ProfileViewModel(DatabaseService dbService, ApiService apiService, IServiceProvider serviceProvider, ILanguageService langService)
    {
        _dbService = dbService;
        _apiService = apiService;
        _serviceProvider = serviceProvider;
        Lang = langService; // Gán giá trị để XAML có thể dùng
        CheckLoginStatus();
    }

    public void CheckLoginStatus()
    {
        string? email = Microsoft.Maui.Storage.Preferences.Default.Get("CurrentUserEmail", "");
        IsLoggedIn = !string.IsNullOrEmpty(email);
    }

    public async Task LoadUserProfileAsync()
    {
        // 🔌 Ưu tiên lấy từ Web API, nếu không được thì dùng local DB
        if (await _apiService.IsWebAdminAvailableAsync())
        {
            Debug.WriteLine("✅ Lấy profile từ Web API");
            string? email = Microsoft.Maui.Storage.Preferences.Default.Get("CurrentUserEmail", "");
            if (!string.IsNullOrEmpty(email))
            {
                CurrentUser = await _apiService.GetUserByEmailAsync(email);
            }
        }

        // Fallback hoặc nếu API không có dữ liệu, lấy từ local DB
        if (CurrentUser == null)
        {
            Debug.WriteLine("⚠️  Lấy profile từ local database");
            CurrentUser = await _dbService.GetCurrentUserAsync();
        }
    }

    [RelayCommand]
    private async Task Login(object? parameter)
    {
        try
        {
            // Lấy email và mật khẩu từ Entry controls
            var emailEntry = (parameter as VerticalStackLayout)?.FindByName<Entry>("EmailEntry");
            var passwordEntry = (parameter as VerticalStackLayout)?.FindByName<Entry>("PasswordEntry");

            if (emailEntry == null || passwordEntry == null)
            {
                await Application.Current?.MainPage?.DisplayAlert("Lỗi", "Không thể tìm thấy trường nhập liệu", "OK");
                return;
            }

            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await Application.Current?.MainPage?.DisplayAlert("Lỗi", "Vui lòng nhập email và mật khẩu", "OK");
                return;
            }

            // 🔌 Ưu tiên Web API, nếu không được thì dùng local DB
            User? user = null;
            if (await _apiService.IsWebAdminAvailableAsync())
            {
                Debug.WriteLine("✅ Đăng nhập từ Web API");
                user = await _apiService.LoginUserAsync(email, password);
            }

            if (user == null)
            {
                Debug.WriteLine("⚠️  Đăng nhập từ local database");
                user = await _dbService.LoginUserAsync(email, password);
            }

            if (user != null)
            {
                // Lưu email vào Preferences
                Microsoft.Maui.Storage.Preferences.Default.Set("CurrentUserEmail", user.Email);
                CurrentUser = user;
                IsLoggedIn = true;
                emailEntry.Text = "";
                passwordEntry.Text = "";
                await Application.Current?.MainPage?.DisplayAlert("Thành công", "Đăng nhập thành công!", "OK");
            }
            else
            {
                await Application.Current?.MainPage?.DisplayAlert("Lỗi", "Email hoặc mật khẩu không chính xác", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Lỗi đăng nhập: {ex.Message}");
            await Application.Current?.MainPage?.DisplayAlert("Lỗi", $"Lỗi đăng nhập: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task Register(object? parameter)
    {
        try
        {
            // Lấy email và mật khẩu từ Entry controls
            var emailEntry = (parameter as VerticalStackLayout)?.FindByName<Entry>("EmailEntry");
            var passwordEntry = (parameter as VerticalStackLayout)?.FindByName<Entry>("PasswordEntry");

            if (emailEntry == null || passwordEntry == null)
            {
                await Application.Current?.MainPage?.DisplayAlert("Lỗi", "Không thể tìm thấy trường nhập liệu", "OK");
                return;
            }

            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await Application.Current?.MainPage?.DisplayAlert("Lỗi", "Vui lòng nhập email và mật khẩu", "OK");
                return;
            }

            // Yêu cầu nhập tên đầy đủ
            string? fullName = await Application.Current?.MainPage?.DisplayPromptAsync(
                "Đăng ký",
                "Nhập tên đầy đủ:",
                "Tiếp tục",
                "Hủy",
                keyboard: Keyboard.Text
            );

            if (string.IsNullOrWhiteSpace(fullName))
            {
                return; // User hủy
            }

            // 🔌 Ưu tiên Web API, nếu không được thì dùng local DB
            bool success = false;
            if (await _apiService.IsWebAdminAvailableAsync())
            {
                Debug.WriteLine("✅ Đăng ký từ Web API");
                success = await _apiService.RegisterUserAsync(fullName, email, password);
            }

             if (!success)
            {
                Debug.WriteLine("⚠️  Đăng ký từ local database");
                success = await _dbService.RegisterUserAsync(fullName, email, password);
            }

            if (success)
            {
                // Tự động đăng nhập sau khi đăng ký
                var user = await _dbService.LoginUserAsync(email, password);
                if (user != null)
                {
                    Microsoft.Maui.Storage.Preferences.Default.Set("CurrentUserEmail", user.Email);
                    CurrentUser = user;
                    IsLoggedIn = true;
                    emailEntry.Text = "";
                    passwordEntry.Text = "";
                    await Application.Current?.MainPage?.DisplayAlert("Thành công", "Đăng ký thành công!", "OK");
                }
            }
            else
            {
                await Application.Current?.MainPage?.DisplayAlert("Lỗi", "Email này đã được đăng ký", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Lỗi đăng ký: {ex.Message}");
            await Application.Current?.MainPage?.DisplayAlert("Lỗi", $"Lỗi đăng ký: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task Logout()
    {
        // Xóa email khỏi Preferences
        Microsoft.Maui.Storage.Preferences.Default.Remove("CurrentUserEmail");

        // Đặt lại trạng thái
        IsLoggedIn = false;
        CurrentUser = null;

        await Application.Current?.MainPage?.DisplayAlert("Thông báo", "Đã đăng xuất", "OK");
    }

    [RelayCommand]
    private async Task EditProfile()
    {
        if (CurrentUser == null)
        {
            await Application.Current?.MainPage?.DisplayAlert("Lỗi", "Chưa tải thông tin người dùng", "OK");
            return;
        }

        // Hiển thị dialog nhập liệu
        string? newFullName = await Application.Current?.MainPage?.DisplayPromptAsync(
            "Chỉnh sửa thông tin",
            "Nhập tên đầy đủ:",
            "Lưu",
            "Hủy",
            CurrentUser.FullName,
            keyboard: Keyboard.Text
        );

        if (string.IsNullOrEmpty(newFullName) || newFullName == CurrentUser.FullName)
        {
            return; // User hủy hoặc không thay đổi
        }

        // Cập nhật tên
        CurrentUser.FullName = newFullName;

        // 🔌 Ưu tiên cập nhật Web API, nếu không được thì cập nhật local DB
        bool isSuccess = false;

        if (await _apiService.IsWebAdminAvailableAsync())
        {
            Debug.WriteLine("✅ Cập nhật profile trên Web API");
            isSuccess = await _apiService.UpdateUserAsync(CurrentUser.Id, CurrentUser);
        }
        else
        {
            Debug.WriteLine("⚠️  Cập nhật profile trên local database");
            isSuccess = await _dbService.UpdateUserAsync(CurrentUser.Id, CurrentUser);
        }

        if (isSuccess)
        {
            await Application.Current?.MainPage?.DisplayAlert("Thành công", "Cập nhật thông tin thành công!", "OK");
        }
        else
        {
            await Application.Current?.MainPage?.DisplayAlert("Lỗi", "Không thể cập nhật thông tin", "OK");
        }
    }

    [RelayCommand]
    private async Task ChangeLanguage()
    {
        if (Application.Current?.MainPage != null)
        {
            // 1. Hiện bảng chọn 4 ngôn ngữ giống hệt bên trang Bản đồ
            string action = await Application.Current.MainPage.DisplayActionSheet("Ngôn ngữ / Language", "Hủy", null, "Tiếng Việt", "English", "日本語", "한국어");

            // Nếu người dùng bấm Hủy hoặc ra ngoài thì bỏ qua
            if (string.IsNullOrEmpty(action) || action == "Hủy") return;

            // 2. Lấy mã ngôn ngữ
            string langCode = "vi";
            if (action == "English") langCode = "en";
            else if (action == "日本語") langCode = "ja";
            else if (action == "한국어") langCode = "ko";

            // 3. Lưu vào database
            if (CurrentUser != null)
            {
                CurrentUser.Language = langCode;
                await _dbService.UpdateUserAsync(CurrentUser.Id, CurrentUser);
            }

            // 4. Ra lệnh cho LanguageService đổi ngôn ngữ TOÀN APP (Nó sẽ tự lưu vào Preferences)
            Lang.ChangeLanguage(langCode);
        }
    }

    [RelayCommand]
    private async Task OpenSettings()
    {
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert("Thông báo", "Tính năng Cài đặt chung đang được phát triển!", "OK");
        }
    }

    public System.Collections.ObjectModel.ObservableCollection<PlayHistory> AudioHistoryList { get; set; } = new();

    public async Task LoadHistoryAsync()
    {
        var list = await _dbService.GetRecentPlayHistoryAsync();
        AudioHistoryList.Clear();
        foreach (var item in list)
        {
            AudioHistoryList.Add(item);
        }
    }
}