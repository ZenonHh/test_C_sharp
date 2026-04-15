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

    // Tiêm các Service vào
    public ProfileViewModel(DatabaseService dbService, ApiService apiService, IServiceProvider serviceProvider, ILanguageService langService)
    {
        _dbService = dbService;
        _apiService = apiService;
        _serviceProvider = serviceProvider;
        Lang = langService; // Gán giá trị để XAML có thể dùng
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
    private void Logout()
    {
        if (Application.Current != null)
        {
            var authPage = _serviceProvider.GetService(typeof(Views.AuthPage)) as Views.AuthPage;
            Application.Current.MainPage = authPage;
        }
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