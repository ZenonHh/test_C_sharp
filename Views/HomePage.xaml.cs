using DoAnCSharp.Services;
using DoAnCSharp.Models;
using DoAnCSharp.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace DoAnCSharp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;
    private readonly ILanguageService _lang;

    // CHỈ DÙNG 1 HÀM KHỞI TẠO DUY NHẤT
    public HomePage(HomeViewModel viewModel, ILanguageService lang)
    {
        InitializeComponent(); // Dòng này giờ sẽ hết lỗi đỏ
        
        _viewModel = viewModel;
        _lang = lang;

        BindingContext = _viewModel; 

        UpdateLanguage();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Load dữ liệu thông qua ViewModel (đã được tiêm dbService bên trong)
        await _viewModel.LoadDataAsync(); 
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
        await _viewModel.LoadDataAsync(); 
    }
}