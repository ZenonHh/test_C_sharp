using Microsoft.Maui.Controls;
using Mapsui;
using Mapsui.Tiling;
using Mapsui.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;

// MỚI: Gọi các thư mục chứa Model và Service vào
using DoAnCSharp.Models;
using DoAnCSharp.Services;

namespace DoAnCSharp.Views;

public partial class MapPage : ContentPage
{
    // MỚI: Khai báo dịch vụ Database và danh sách chứa dữ liệu
    private DatabaseService _dbService = new DatabaseService();
    private List<AudioPOI> _pois = new();

    private IDispatcherTimer? _radarTimer;
    private CancellationTokenSource? _ttsCancellationTokenSource;
    private AudioPOI? _currentPoi;
    private bool _isPlaying = false;
    private bool _isManualSelection = false; 
    private string _targetLang = "vi"; 

    public MapPage()
    {
        try 
        {
            InitializeComponent();
            SetupMap();
            StartRadar();
            // Lưu ý: Mình không gọi LoadPinsToMap ở đây nữa vì phải đợi Database nạp xong mới có Pin để vẽ
        }
        catch (Exception ex)
        {
            Dispatcher.Dispatch(async () => await DisplayAlert("LỖI KHỞI TẠO", ex.Message, "OK"));
        }
    }

    // MỚI: Hàm OnAppearing sẽ tự động chạy khi trang Bản đồ vừa hiển thị lên màn hình
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataFromDatabaseAsync();
    }

    // MỚI: Hàm "hút" dữ liệu từ SQLite
    private async Task LoadDataFromDatabaseAsync()
    {
        try
        {
            // Lấy toàn bộ quán ốc từ CSDL
            _pois = await _dbService.GetPOIsAsync();
            
            // Có dữ liệu rồi mới đem ghim lên bản đồ
            LoadPinsToMap();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi Dữ Liệu", "Không thể tải dữ liệu quán ăn: " + ex.Message, "OK");
        }
    }

    private async Task<string> TranslateTextAsync(string text, string toLang)
    {
        if (string.IsNullOrEmpty(text) || toLang == "vi") return text;
        try {
            string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=vi&tl={toLang}&dt=t&q={Uri.EscapeDataString(text)}";
            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var json = JsonDocument.Parse(response);
            return json.RootElement[0][0][0].GetString() ?? text;
        } catch { return text; }
    }

    private void SetupMap()
    {
        if (foodMapView.Map != null) {
            foodMapView.Map.Layers.Add(OpenStreetMap.CreateTileLayer("VTour"));
            var center = SphericalMercator.FromLonLat(106.7000, 10.7600);
            foodMapView.Map.Home = n => n.CenterOnAndZoomTo(new MPoint(center.x, center.y), 2);
        }
        foodMapView.MyLocationEnabled = true;
    }

    private void StartRadar()
    {
        _radarTimer = Dispatcher.CreateTimer();
        _radarTimer.Interval = TimeSpan.FromSeconds(3);
        _radarTimer.Tick += async (s, e) => await CheckGeofenceAndPlayAudio();
        _radarTimer.Start();
    }

    // --- LOGIC GPS THỰC TẾ ---
    private async Task CheckGeofenceAndPlayAudio()
    {
        try {
            if (_isManualSelection) return;

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(2));
            var userLoc = await Geolocation.Default.GetLocationAsync(request);

            if (userLoc == null) return; 

            MainThread.BeginInvokeOnMainThread(() => {
                foodMapView.MyLocationLayer?.UpdateMyLocation(new Mapsui.UI.Maui.Position(userLoc.Latitude, userLoc.Longitude));
            });

            var poi = _pois.FirstOrDefault(p => Location.CalculateDistance(userLoc, new Location(p.Lat, p.Lng), DistanceUnits.Kilometers) * 1000 <= p.Radius);

            if (poi != null) {
                if (_currentPoi != poi) {
                    PlayAudioAlert(poi);
                }
            } else {
                if (_currentPoi != null && !_isManualSelection) {
                    StopAudio();
                    _currentPoi = null;
                }
            }
        } catch { }
    }

    private void StopAudio()
    {
        _ttsCancellationTokenSource?.Cancel();
        _isPlaying = false;
        _isManualSelection = false;
        MainThread.BeginInvokeOnMainThread(() => {
            AudioPlayerUI.IsVisible = false;
            PlayStopButton.Text = "▶️";
        });
    }

    private async void OnLanguageClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Ngôn ngữ / Language", "Hủy", null, "Tiếng Việt", "English", "日本語", "한국어");
        if (string.IsNullOrEmpty(action) || action == "Hủy") return;

        if (action == "Tiếng Việt") _targetLang = "vi";
        else if (action == "English") _targetLang = "en";
        else if (action == "日本語") _targetLang = "ja";
        else if (action == "한국어") _targetLang = "ko";

        if (_currentPoi != null) PlayAudioAlert(_currentPoi);
    }

    private async void PlayAudioAlert(AudioPOI poi)
    {
        _currentPoi = poi;
        _isPlaying = true;

        MainThread.BeginInvokeOnMainThread(() => {
            TranslationLoader.IsVisible = true;
            TranslationLoader.IsRunning = true;
            AudioText.Text = _targetLang == "vi" ? poi.Name : "Translating...";
            AudioPlayerUI.IsVisible = true;
            PoiDetailCard.IsVisible = false; 
            PlayStopButton.Text = "⏹";
        });

        string tName = await TranslateTextAsync(poi.Name, _targetLang);
        string tDesc = await TranslateTextAsync(poi.Description, _targetLang);

        MainThread.BeginInvokeOnMainThread(() => { 
            TranslationLoader.IsRunning = false;
            TranslationLoader.IsVisible = false;
            AudioText.Text = tName; 
            AudioStatusLabel.Text = _targetLang == "vi" ? "Đang phát review:" : "Playing review:"; 
        });

        _ttsCancellationTokenSource?.Cancel();
        _ttsCancellationTokenSource = new CancellationTokenSource();

        try {
            var locales = await TextToSpeech.Default.GetLocalesAsync();
            Locale? locale = null;

            if (_targetLang == "en") locale = locales.FirstOrDefault(l => l.Language.Equals("en-US", StringComparison.OrdinalIgnoreCase)) ?? locales.FirstOrDefault(l => l.Language.StartsWith("en", StringComparison.OrdinalIgnoreCase));
            else if (_targetLang == "ja") locale = locales.FirstOrDefault(l => l.Language.Equals("ja-JP", StringComparison.OrdinalIgnoreCase)) ?? locales.FirstOrDefault(l => l.Language.StartsWith("ja", StringComparison.OrdinalIgnoreCase));
            else if (_targetLang == "ko") locale = locales.FirstOrDefault(l => l.Language.Equals("ko-KR", StringComparison.OrdinalIgnoreCase)) ?? locales.FirstOrDefault(l => l.Language.StartsWith("ko", StringComparison.OrdinalIgnoreCase));
            else locale = locales.FirstOrDefault(l => l.Language.Equals("vi-VN", StringComparison.OrdinalIgnoreCase)) ?? locales.FirstOrDefault(l => l.Language.StartsWith("vi", StringComparison.OrdinalIgnoreCase));

            await TextToSpeech.Default.SpeakAsync(tDesc, new SpeechOptions { Locale = locale }, _ttsCancellationTokenSource.Token);
        } catch { }
        finally {
            _isPlaying = false;
            MainThread.BeginInvokeOnMainThread(() => { PlayStopButton.Text = "▶️"; });
        }
    }

    private void OnMapPinClicked(object? sender, Mapsui.UI.Maui.PinClickedEventArgs e)
    {
        if (e.Pin?.Tag is AudioPOI clickedPoi)
        {
            _isManualSelection = true;
            StopAudio(); 
            _ = UpdateDetailCardAsync(clickedPoi); 
        }
        e.Handled = true;
    }

    private async Task UpdateDetailCardAsync(AudioPOI poi)
    {
        _currentPoi = poi; 

        MainThread.BeginInvokeOnMainThread(() => {
            DetailName.Text = "Translating...";
            PoiDetailCard.IsVisible = true;
            AudioPlayerUI.IsVisible = false; 
        });

        string tName = await TranslateTextAsync(poi.Name, _targetLang);
        string tDesc = await TranslateTextAsync(poi.Description, _targetLang);

        MainThread.BeginInvokeOnMainThread(() => {
            DetailName.Text = tName;
            DetailDescription.Text = tDesc;
            DetailImage.Source = poi.ImageAsset;
        });
    }

    private void LoadPinsToMap()
    {
        if (foodMapView == null) return;
        
        foodMapView.Pins.Clear();
        foreach (var poi in _pois) {
            foodMapView.Pins.Add(new Mapsui.UI.Maui.Pin(foodMapView) { Label = poi.Name, Position = new Mapsui.UI.Maui.Position(poi.Lat, poi.Lng), Tag = poi, Color = Microsoft.Maui.Graphics.Colors.Red });
        }
        foodMapView.PinClicked -= OnMapPinClicked;
        foodMapView.PinClicked += OnMapPinClicked;
    }

    private void CustomMyLocationClicked(object? sender, EventArgs e)
    {
        var center = SphericalMercator.FromLonLat(106.7000, 10.7600);
        foodMapView.Map?.Navigator.CenterOnAndZoomTo(new MPoint(center.x, center.y), 2);
    }

    private void ToggleAudioClicked(object? sender, EventArgs e) {
        if (_isPlaying) StopAudio(); 
        else if (_currentPoi != null) PlayAudioAlert(_currentPoi);
    }

    private void CloseDetailClicked(object? sender, EventArgs e) {
        PoiDetailCard.IsVisible = false;
        _isManualSelection = false; 
    }

    private void PlayReviewFromDetailClicked(object? sender, EventArgs e) {
        if (_currentPoi != null) { 
            _isManualSelection = true;
            PoiDetailCard.IsVisible = false; 
            PlayAudioAlert(_currentPoi); 
        }
    }
}