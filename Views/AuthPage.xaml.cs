using DoAnCSharp.ViewModels;
using Microsoft.Maui.Controls;

namespace DoAnCSharp.Views;

public partial class AuthPage : ContentPage
{
    public AuthPage(AuthViewModel viewModel) // Nhận não vào đây
    {
        InitializeComponent();
        BindingContext = viewModel; // "Nối mạch" não với xác
    }
    
    private async void OnForgotPasswordTapped(object sender, TappedEventArgs e)
    {
        // ĐÃ SỬA: Dùng PushModalAsync thay vì PushAsync để tránh văng app do thiếu NavigationPage
        await Navigation.PushModalAsync(new ForgotPasswordPage());
    }
}