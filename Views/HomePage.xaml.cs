using DoAnCSharp.Services;
using DoAnCSharp.Models;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace DoAnCSharp.Views;

public partial class HomePage : ContentPage
{
    // 1. Khai báo DatabaseService mới thay cho repo cũ
    private DatabaseService _dbService = new DatabaseService();
    private readonly ILanguageService _lang;

    // 2. XÓA IPoiRepository ra khỏi ngoặc, chỉ giữ lại ILanguageService
    public HomePage(ILanguageService lang)
    {
        InitializeComponent();
        _lang = lang;

        UpdateLanguage();
        // Không gọi LoadData() ở đây nữa vì nó là hàm Async
    }

    // 3. Gọi dữ liệu khi trang vừa hiện lên (Chuẩn bài MAUI)
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync();
    }

    // 4. Sửa thành hàm Async để hút dữ liệu từ SQLite
    private async Task LoadDataAsync()
    {
        // Lấy danh sách từ CSDL
        var pois = await _dbService.GetPOIsAsync();
        
        // Đổ dữ liệu vào giao diện
        PoiCollectionView.ItemsSource = pois;
    }

    private void UpdateLanguage()
    {
        WelcomeLabel.Text = $"{_lang.T("welcome")} Gastronome! 👋";
        RecommendTitle.Text = _lang.T("recommend");
    }

    private async void OnChangeLangClicked(object sender, EventArgs e)
    {
        string newLang = _lang.CurrentLocale == "vi" ? "en" : "vi";
        _lang.ChangeLanguage(newLang);
        UpdateLanguage();
        
        // Đổi ngôn ngữ xong thì gọi lại hàm Async để nạp lại danh sách
        await LoadDataAsync(); 
    }
}