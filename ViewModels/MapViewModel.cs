using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoAnCSharp.Models;
using DoAnCSharp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoAnCSharp.ViewModels;

public partial class MapViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private readonly ApiService _apiService;
    private readonly ILanguageService _langService;

    private ObservableCollection<AudioPOI> _poisList = new();
    public ObservableCollection<AudioPOI> PoisList => _poisList;

    private bool _isLoading = false;
    public bool IsLoading 
    { 
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private string _currentLanguage = "vi";
    public string CurrentLanguage
    {
        get => _currentLanguage;
        set => SetProperty(ref _currentLanguage, value);
    }

    public MapViewModel(DatabaseService dbService, ApiService apiService, ILanguageService langService)
    {
        _dbService = dbService;
        _apiService = apiService;
        _langService = langService;
        _currentLanguage = _langService.CurrentLocale;
    }

    public async Task LoadPOIsAsync()
    {
        try
        {
            IsLoading = true;
            Debug.WriteLine("📍 Loading POIs...");

            // 🔌 Ưu tiên lấy từ Web API, nếu không được thì dùng local DB
            List<AudioPOI> pois = null;

            if (await _apiService.IsWebAdminAvailableAsync())
            {
                Debug.WriteLine("✅ Loading POIs from Web API");
                pois = await _apiService.GetPOIsAsync();
            }
            else
            {
                Debug.WriteLine("⚠️  Loading POIs from local database");
                pois = await _dbService.GetPOIsAsync();
            }

            // Cập nhật UI
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _poisList.Clear();
                foreach (var poi in pois ?? new())
                {
                    _poisList.Add(poi);
                }
                Debug.WriteLine($"✅ Loaded {_poisList.Count} POIs");
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ LoadPOIs Error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ChangeLanguage(string langCode)
    {
        CurrentLanguage = langCode;
        _langService.ChangeLanguage(langCode);
    }
}