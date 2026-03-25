using CommunityToolkit.Mvvm.ComponentModel;
using DoAnCSharp.Services;
using DoAnCSharp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DoAnCSharp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;

    // Tự động tạo property AllPois để giao diện binding
    [ObservableProperty]
    private ObservableCollection<AudioPOI> _allPois = new();

    // Nhận DatabaseService từ hệ thống
    public HomeViewModel(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    // Hàm gọi dữ liệu từ DB (HomePage.xaml.cs đang đợi hàm này)
    public async Task LoadDataAsync()
    {
        var items = await _dbService.GetPOIsAsync();
        AllPois = new ObservableCollection<AudioPOI>(items);
    }
}