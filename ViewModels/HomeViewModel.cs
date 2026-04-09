#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAnCSharp.Models;
using DoAnCSharp.Services;
using Microsoft.Maui.Devices.Sensors;
using System;

namespace DoAnCSharp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private List<AudioPOI> _originalPois = new();

    public ILanguageService Lang { get; }

    // ==========================================
    // 1. CÁC BIẾN THÔNG TIN NGƯỜI DÙNG (ĐÃ FIX LỖI Ở ĐÂY)
    // ==========================================
    [ObservableProperty]
    private string _userName = "Khách";

    [ObservableProperty]
    private string _userImage = "dotnet_bot.png";

    [ObservableProperty]
    private string _welcomeMessage = "";


    // ==========================================
    // 2. CÁC BIẾN QUẢN LÝ DANH SÁCH QUÁN ĂN
    // ==========================================
    public ObservableCollection<AudioPOI> RecommendedPois { get; set; } = new();
    public ObservableCollection<AudioPOI> AllPois { get; set; } = new();

    [ObservableProperty]
    private bool _isRecommendedVisible = true;

    [ObservableProperty]
    private string _searchResultTitle = "Tất cả quán ăn";


    // ==========================================
    // 3. CÁC BIẾN QUẢN LÝ LỊCH SỬ TÌM KIẾM
    // ==========================================
    public ObservableCollection<string> SearchHistory { get; set; } = new();

    [ObservableProperty]
    private bool _isSearchHistoryVisible = false;


    public HomeViewModel(DatabaseService dbService, ILanguageService languageService)
    {
        _dbService = dbService;
        Lang = languageService;
    }

    public async Task LoadDataAsync()
    {
        // 1. Lấy thông tin User để hiển thị lên Header
        var currentUser = await _dbService.GetCurrentUserAsync();

        if (currentUser != null)
        {
            UserName = string.IsNullOrWhiteSpace(currentUser.FullName) ? currentUser.Email : currentUser.FullName;
            UserImage = currentUser.Avatar;
        }
        else
        {
            UserName = "Khách";
            UserImage = "dotnet_bot.png";
        }

        WelcomeMessage = $"Chào {UserName}!";

        // 2. Lấy dữ liệu quán ăn
        var data = await _dbService.GetPOIsAsync();
        if (data != null)
        {
            _originalPois = data;
            await CalculateDistancesAsync();
        }
        FilterList("");
    }

    private async Task CalculateDistancesAsync()
    {
        try
        {
            var userLocation = await Geolocation.Default.GetLastKnownLocationAsync();
            if (userLocation == null)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(2));
                userLocation = await Geolocation.Default.GetLocationAsync(request);
            }

            if (userLocation != null)
            {
                foreach (var poi in _originalPois)
                {
                    double distanceKm = Location.CalculateDistance(userLocation, poi.Lat, poi.Lng, DistanceUnits.Kilometers);
                    string distStr = distanceKm < 1 ? $"{(int)(distanceKm * 1000)}m" : $"{Math.Round(distanceKm, 1)}km";
                    int walkMinutes = Math.Max(1, (int)(distanceKm * 12));
                    poi.DistanceInfo = $"📍 {distStr}  •  🚶 {walkMinutes} phút";
                }
            }
        }
        catch
        {
            foreach (var poi in _originalPois)
            {
                poi.DistanceInfo = "📍 Chưa có định vị";
            }
        }
    }

    public void FilterList(string query)
    {
        query = query?.ToLower() ?? "";
        RecommendedPois.Clear();
        AllPois.Clear();

        if (string.IsNullOrWhiteSpace(query) || query == "phổ biến")
        {
            IsRecommendedVisible = true;
            SearchResultTitle = Lang.CurrentLocale == "vi" ? "Tất cả quán ăn" : "All restaurants";

            // Lấy 2 quán nổi bật nhất cho phần Đề xuất ở trên cùng
            var topPois = _originalPois.OrderByDescending(p => p.Priority).Take(2).ToList();
            foreach (var item in topPois) RecommendedPois.Add(item);

            // ĐÃ SỬA LỖI Ở ĐÂY: Thay vì chỉ lấy 3 quán (Take(3)), bây giờ lấy TẤT CẢ các quán còn lại
            // để khi người dùng bấm về "Phổ biến", danh sách sẽ hiện đầy đủ không bị trống
            var otherPois = _originalPois.Where(p => !topPois.Contains(p)).ToList();
            foreach (var item in otherPois) AllPois.Add(item);
        }
        else
        {
            IsRecommendedVisible = false;
            SearchResultTitle = Lang.CurrentLocale == "vi" ? $"Kết quả tìm kiếm cho '{query}'" : $"Search results for '{query}'";

            var filtered = _originalPois.Where(p =>
                (p.Name != null && p.Name.ToLower().Contains(query)) ||
                (p.Description != null && p.Description.ToLower().Contains(query)) ||
                (query.Contains("ốc") && p.Name != null && p.Name.ToLower().Contains("ốc")) ||
                (query.Contains("lẩu") && p.Name != null && p.Name.ToLower().Contains("lẩu"))
            ).ToList();

            foreach (var item in filtered) AllPois.Add(item);
        }
    }

    public void LoadSearchHistory()
    {
        var historyStr = Microsoft.Maui.Storage.Preferences.Default.Get("SearchHistoryLog", "");
        var list = historyStr.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
        SearchHistory.Clear();
        foreach (var item in list) SearchHistory.Add(item);
    }

    public void AddSearchHistory(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return;
        var list = SearchHistory.ToList();
        list.Remove(query);
        list.Insert(0, query); // Đưa lên đầu
        if (list.Count > 5) list = list.Take(5).ToList(); // Lưu tối đa 5 từ khóa

        Microsoft.Maui.Storage.Preferences.Default.Set("SearchHistoryLog", string.Join("|", list));
        LoadSearchHistory();
    }


}