using VinhKhanhFoodTour.ViewModels;

namespace VinhKhanhFoodTour.Views;

public partial class AuthPage : ContentPage
{
    public AuthPage(AuthViewModel viewModel) // Nhận não vào đây
    {
        InitializeComponent();
        BindingContext = viewModel; // "Nối mạch" não với xác
    }
}